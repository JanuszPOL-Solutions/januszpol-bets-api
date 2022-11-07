using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Data.Entities;
using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using JanuszPOL.JanuszPOLBets.Repository.Events.Dto;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using Microsoft.EntityFrameworkCore;

namespace JanuszPOL.JanuszPOLBets.Repository.Events
{
    public interface IEventsRepository
    {
        // We're assuming that there won't be a lot of events so we don't need to paginate it
        Task<IEnumerable<EventDto>> GetEvents();
        Task<EventDto> GetEvent(long eventId);
        Task<Event> GetEventById(long eventId);
        Task<GameEventBetDto> AddEventBet(AddEventBetDto addEventBetDto);
        Task<GameEventBetDto> UpdateEventBet(ExistingEventBetDto updateEventBetDto);
        Task<IEnumerable<ExistingEventBetDto>> GetEventBetsForGameAndUser(long gameId, long accountId);
        Task<IEnumerable<ExistingEventBetDto>> GetBetsForGame(long gameId);
        Task AddEventBetResult(AddEventBetResultDto addEventBetResultDto);
        Task<IEnumerable<EventBetResultDto>> GetUserBets(long accountId);
        Task<ExistingEventBetDto> GetEventBet(long betId, long accountId);
        Task DeleteEventBet(long betId);
        Task UpdateBaseBet(long gameId, int Team1Score, int Team2Score);
        Task UpdateBoolBet(long gameId, long eventId, bool eventHappened);
        Task UpdateSingleExactBet(long gameId, int Team1Score, int Team2Score);
        Task UpdateBothExactBet(long gameId, int Team1Score, int Team2Score);



        long Team1WinEventId { get; }
        long Team2WinEventId { get; }
        long TieEventId { get; }
        Task<RankingDto> GetFullRanking();
    }

    public class EventsRepository : IEventsRepository
    {
        private readonly DataContext _dataContext;

        public EventsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Let's just hardcode it here for now, later we should take it from DB
        public long Team1WinEventId => 1;
        public long Team2WinEventId => 2;
        public long TieEventId => 3;

        public async Task<GameEventBetDto> AddEventBet(AddEventBetDto addEventBetDto)
        {
            var savedBet = _dataContext.EventBet.Add(new EventBet
            {
                AccountId = addEventBetDto.AccountId,
                EventId = addEventBetDto.EventId,
                GameId = addEventBetDto.GameId,
                Value1 = addEventBetDto.Value1,
                Value2 = addEventBetDto.Value2
            });

            await _dataContext.SaveChangesAsync();

            long? matchResult = null;
            if (savedBet.Entity.EventId <= 3) //base bet
            {
                matchResult = savedBet.Entity.EventId;
            }

            return new GameEventBetDto
            {
                Id = savedBet.Entity.Id,
                EventId = savedBet.Entity.EventId,
                EventTypeId = savedBet.Entity.Event.EventTypeId,
                Team1Score = savedBet.Entity.Value1,
                Team2Score = savedBet.Entity.Value2,
                Name = savedBet.Entity.Event.Name,
                GainedPoints = 0,
                BetCost = savedBet.Entity.Event.BetCost,
                MatchResult = matchResult
            };
        }

        public async Task<EventDto> GetEvent(long eventId)
        {
            var @event = await _dataContext.Event
                .Where(x => x.Id == eventId)
                .Select(x => TranslateToEventDto(x))
                .SingleAsync();

            return @event;
        }

        public async Task<IEnumerable<ExistingEventBetDto>> GetEventBetsForGameAndUser(long gameId, long accountId)
        {
            var bets = await _dataContext.EventBet
                .Include(x => x.Event)
                .Where(x => !x.IsDeleted)
                .Where(x => x.AccountId == accountId && x.GameId == gameId && !x.IsDeleted)
                .Select(x => TranslateToExistingBetDto(x))
                .ToListAsync();

            return bets;
        }

        public async Task<IEnumerable<EventDto>> GetEvents()
        {
            var events = await _dataContext.Event
                .Select(x => TranslateToEventDto(x))
                .ToListAsync();

            return events;
        }

        public async Task<GameEventBetDto> UpdateEventBet(ExistingEventBetDto updateEventBetDto)
        {
            var bet = await _dataContext.EventBet
                .SingleAsync(x => x.Id == updateEventBetDto.EventBetId && !x.IsDeleted);

            bet.EventId = updateEventBetDto.EventId; ;
            bet.AccountId = updateEventBetDto.AccountId;
            bet.GameId = updateEventBetDto.GameId;
            bet.Value1 = updateEventBetDto.Value1;
            bet.Value2 = updateEventBetDto.Value2;

            _dataContext.SaveChanges();


            long? matchResult = null;
            if (bet.EventId <= 3) //base bet
            {
                matchResult = bet.EventId;
            }

            return new GameEventBetDto
            {
                Id = bet.Id,
                EventId = bet.EventId,
                EventTypeId = bet.Event.EventTypeId,
                Team1Score = bet.Value1,
                Team2Score = bet.Value2,
                Name = bet.Event.Name,
                GainedPoints = 0,
                BetCost = bet.Event.BetCost,
                MatchResult = matchResult
            };
        }

        public async Task<IEnumerable<ExistingEventBetDto>> GetBetsForGame(long gameId)
        {
            var bets = await _dataContext.EventBet
                .Where(x => x.GameId == gameId && !x.IsDeleted)
                //.Select(x => TranslateToExistingBetDto(x))
                .Select(x => new ExistingEventBetDto
                {
                    EventBetId = x.Id,
                    AccountId = x.AccountId,
                    GameId = x.GameId,
                    EventId = x.EventId,
                    Value1 = x.Value1,
                    Value2 = x.Value2,
                    EventName = x.Event.Name,
                    EventDescription = x.Event.Description,
                    EventType = x.Event.EventTypeId
                })
                .ToListAsync();

            return bets;
        }

        public async Task AddEventBetResult(AddEventBetResultDto addEventBetResultDto)
        {
            var bets = await _dataContext.EventBet
                .Include(x => x.Event)
                .Where(x => !x.IsDeleted && addEventBetResultDto.EventBetIds.Contains(x.Id))
                .ToListAsync();

            if (bets == null)
            {
                return;
            }

            foreach (var bet in bets)
            {
                bet.Result = true;
            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventBetResultDto>> GetUserBets(long accountId)
        {
            var userCorrectBaseBets = await _dataContext
                .EventBet
                .Where(x => x.AccountId == accountId && !x.IsDeleted)
                .Select(x => new EventBetResultDto
                {
                    AccountId = x.AccountId,
                    EventId = x.EventId,
                    GameId = x.GameId,
                    Result = x.Result,
                    Value1 = x.Value1,
                    Value2 = x.Value2,
                    BetCost = x.Event.BetCost,
                    WinValue = x.Event.WinValue
                })
                .ToListAsync();

            return userCorrectBaseBets;
        }

        public async Task DeleteEventBet(long betId)
        {
            var bet = await _dataContext.EventBet.SingleAsync(x => x.Id == betId);

            if (bet.IsDeleted)
            {
                throw new Exception("Event bet already deleted");
            }

            bet.IsDeleted = true;

            await _dataContext.SaveChangesAsync();
        }

        public async Task<ExistingEventBetDto> GetEventBet(long betId, long accountId)
        {
            var bet = await _dataContext
                .EventBet
                .Where(x => x.Id == betId && !x.IsDeleted)
                .Where(x => x.AccountId == accountId)
                .Select(x => new ExistingEventBetDto
                {
                    EventId = x.EventId,
                    AccountId = x.AccountId,
                    EventBetId = x.Id,
                    GameId = x.GameId,
                    Value1 = x.Value1,
                    Value2 = x.Value2
                })
                .SingleAsync();

            return bet;
        }

        public async Task<RankingDto> GetFullRanking()
        {
            var allEventBets = await _dataContext.EventBet
                .Where(x => !x.IsDeleted)
                .Select(x => new
                {
                    x.AccountId,
                    x.Account.UserName,
                    x.Event.WinValue,
                    x.Event.BetCost,
                    x.Result
                })
                .ToListAsync();


            var ranking = allEventBets
                .GroupBy(x => x.AccountId)
                .ToDictionary(x => x.First().UserName, x => x)
                .Select(x => new RankingDto.RankingRow
                {
                    Username = x.Key,
                    Score = //TODO: add base value
                    x.Value.Where(y => y.Result == true).Sum(y => y.WinValue)
                    -
                    x.Value.Sum(y => y.BetCost)
                });

            return new RankingDto
            {
                Rows = ranking.ToArray()
            };
        }

        private static EventDto TranslateToEventDto(Event evt)
        {
            return new EventDto
            {
                BetCost = evt.BetCost,
                Description = evt.Description,
                EventType = evt.EventTypeId,
                Id = evt.Id,
                Name = evt.Name,
                WinValue = evt.WinValue
            };
        }

        private static ExistingEventBetDto TranslateToExistingBetDto(EventBet eventBet)
        {
            return new ExistingEventBetDto
            {
                EventBetId = eventBet.Id,
                AccountId = eventBet.AccountId,
                GameId = eventBet.GameId,
                EventId = eventBet.EventId,
                Value1 = eventBet.Value1,
                Value2 = eventBet.Value2,
                EventName = eventBet.Event.Name,
                EventDescription = eventBet.Event.Description,
                EventType = eventBet.Event.EventTypeId
            };
        }

        public async Task UpdateBaseBet(long gameId, int Team1Score, int Team2Score)
        {
            var resultEventId = 0;
            if (Team1Score > Team2Score)
            {
                resultEventId = 1;
            }
            else if (Team1Score < Team2Score)
            {
                resultEventId = 2;
            }
            else
            {
                resultEventId = 3;
            }

            var baseBetEvents = await _dataContext.EventBet.Where(x => x.GameId == gameId).ToListAsync();
                
            foreach(var bet in baseBetEvents)
            {
                bet.Result = resultEventId == bet.EventId;
            }
            await _dataContext.SaveChangesAsync();
        }
        public async Task UpdateSingleExactBet(long gameId, int Team1Score, int Team2Score)
        {
            var score = Team1Score + Team2Score;
            await _dataContext.EventBet.Where(x => x.GameId == gameId).Where(x => x.Event.Id == 7).ForEachAsync(x =>
            {
                var result = x.Value1 >= score;
                x.Result = result;
            });

           await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateBothExactBet(long gameId, int Team1Score, int Team2Score)
        {
            await _dataContext.EventBet.Where(x => x.GameId == gameId).Where(x => x.Event.Id == 8).ForEachAsync(x =>
            {
                var result = x.Value1 == Team1Score && x.Value2 == Team2Score;
                x.Result = result;
            });

            await _dataContext.SaveChangesAsync();

        }

        public async Task UpdateBoolBet(long gameId, long eventId, bool eventHappened)
        {
            await _dataContext.EventBet.Where(x => x.GameId == gameId).Where(x => x.EventId == eventId)
                .ForEachAsync(x => 
                {
                    x.Result = eventHappened;
                });
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Event> GetEventById(long eventId)
        {
            return await _dataContext.Event.SingleAsync(x => x.Id == eventId);
        }
    }
}

