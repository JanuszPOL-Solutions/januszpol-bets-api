using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using JanuszPOL.JanuszPOLBets.Repository.Events.Dto;
using Microsoft.EntityFrameworkCore;

namespace JanuszPOL.JanuszPOLBets.Repository.Events
{
    public interface IEventsRepository
    {
        // We're assuming that there won't be a lot of events so we don't need to paginate it
        Task<IEnumerable<EventDto>> GetEvents();
        Task<EventDto> GetEvent(long eventId);
        Task AddEventBet(AddEventBetDto addEventBetDto);
        Task UpdateEventBet(ExistingEventBetDto updateEventBetDto);
        Task<IEnumerable<ExistingEventBetDto>> GetEventBetsForGameAndUser(long gameId, long accountId);
        Task<IEnumerable<ExistingEventBetDto>> GetBaseEventBetsForGame(long gameId);
        Task AddEventBetResult(AddEventBetResultDto addEventBetResultDto);
        Task<IEnumerable<EventBetDto>> GetUserBaseBets(long accountId);
        Task UpdateEventBase(EventBetDto updateEventBetDto);
        Task UpdatePenalties(EventBetDto updateEventBetDto);
        Task UpdateOvertime(EventBetDto updateEventBetDto);
        Task UpdateAtLeast(EventBetDto updateEventBetDto);
        Task UpdateExact(EventBetDto updateEventBetDto);
        Task UpdateScoreUnder(EventBetDto updateEventBetDto);



        long Team1WinEventId { get; }
        long Team2WinEventId { get; }
        long TieEventId { get; }
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

        public async Task AddEventBet(AddEventBetDto addEventBetDto)
        {
            var evt = _dataContext.EventBet.Add(new EventBet
            {
                AccountId = addEventBetDto.AccountId,
                EventId = addEventBetDto.EventId,
                GameId = addEventBetDto.GameId,
                Value1 = addEventBetDto.Value1,
                Value2 = addEventBetDto.Value2
            });

            _dataContext.SaveChanges();
        }

        public async Task<EventDto> GetEvent(long eventId)
        {
            return await Task.FromResult(
                TranslateToEventDto(
                    _dataContext
                    .Event
                    .Include(x => x.EventType)
                    .Single(x => x.Id == eventId)));
        }

        public async Task<IEnumerable<ExistingEventBetDto>> GetEventBetsForGameAndUser(long gameId, long accountId)
        {
            return await Task.FromResult(
                _dataContext
                .EventBet
                .Where(x => x.AccountId == accountId && x.GameId == gameId)
                ?.Select(TranslateToExistingBetDto));
        }

        public async Task<IEnumerable<EventDto>> GetEvents()
        {
            return await Task.FromResult(
                _dataContext
                .Event
                .Include(x => x.EventType)
                .Select(TranslateToEventDto)
                .ToList());
        }

        public async Task UpdateEventBet(ExistingEventBetDto updateEventBetDto)
        {
            var bet = _dataContext.EventBet.Single(x => x.Id == updateEventBetDto.EventBetId);
            bet.EventId = updateEventBetDto.EventId; ;
            bet.AccountId = updateEventBetDto.AccountId;
            bet.GameId = updateEventBetDto.GameId;
            bet.Value1 = updateEventBetDto.Value1;
            bet.Value2 = updateEventBetDto.Value2;
            bet.Result = updateEventBetDto.Result;

            _dataContext.SaveChanges();
        }

        public async Task<IEnumerable<ExistingEventBetDto>> GetBaseEventBetsForGame(long gameId)
        {
            var bets = _dataContext
                .EventBet
                .Where(x => x.EventId == Team1WinEventId ||
                    x.EventId == Team2WinEventId ||
                    x.EventId == TieEventId);

            if (bets == null)
            {
                return null;
            }

            return bets.Select(TranslateToExistingBetDto);
        }

        public async Task AddEventBetResult(AddEventBetResultDto addEventBetResultDto)
        {
            var bets = _dataContext
                .EventBet
                .Where(x => addEventBetResultDto
                    .EventBetIds
                    .Any(y => y == x.Id));

            if (bets == null)
            {
                return;
            }

            foreach(var bet in bets)
            {
                bet.Result = true;
            }

            _dataContext.SaveChanges();
        }

        public async Task<IEnumerable<EventBetDto>> GetUserBaseBets(long accountId)
        {
            var userCorrectBaseBets = _dataContext
                .EventBet
                .Where(
                    x => x.AccountId == accountId && 
                    (x.EventId == Team1WinEventId ||
                    x.EventId == Team2WinEventId ||
                    x.EventId == TieEventId));

            return userCorrectBaseBets == null ?
                null :
                userCorrectBaseBets.Select(x => new EventBetDto
                {
                    AccountId = x.AccountId,
                    EventId = x.EventId,
                    GameId = x.GameId,
                    Result = x.Result,
                    Value1 = x.Value1,
                    Value2 = x.Value2
                });
        }

        private EventDto TranslateToEventDto(Event evt)
        {
            return new EventDto
            {
                BetCost = evt.BetCost,
                Description = evt.Description,
                EventType = EventDto.TranslateEventType(evt.EventType),
                Id = evt.Id,
                Name = evt.Name,
                WinValue = evt.WinValue
            };
        }

        private ExistingEventBetDto TranslateToExistingBetDto(EventBet eventBet)
        {
            return new ExistingEventBetDto
            {
                EventBetId = eventBet.Id,
                AccountId = eventBet.AccountId,
                GameId = eventBet.GameId,
                EventId = eventBet.EventId,
                Value1 = eventBet.Value1,
                Value2 = eventBet.Value2
            };
        }

        public async Task UpdateEventBase(EventBetDto updateEventBetDto)
        {
            int[] eventsIds = { 1, 2, 3, 4, 5 };
            ClearEventResult(eventsIds);

            var eventsToBeUpdated = _dataContext.EventBet.Where(x => x.EventId == updateEventBetDto.EventId).Where(x => x.GameId == updateEventBetDto.GameId).ToList();

            eventsToBeUpdated.ForEach(x => x.Result = true);
            
            _dataContext.SaveChanges();
        }
        public async Task UpdatePenalties(EventBetDto updateEventBetDto)
        {
            int[] eventsIds = { 4, 5 };
            ClearEventResult(eventsIds);

            var listUsersId = _dataContext.Users.Select(x => x.Id).ToListAsync();

            foreach (var userId in await listUsersId)
            {
                var eventsToBeUpdated = await _dataContext.EventBet
                        .Where(x => x.EventId == updateEventBetDto.EventId)
                        .Where(x => x.GameId == updateEventBetDto.GameId)
                        .Where(x => x.AccountId == userId)
                        .ToListAsync();

                var userBetCorrect = _dataContext.EventBet
                    .Where(x => x.GameId == updateEventBetDto.GameId)
                    .Where(x => x.AccountId == userId)
                    .Where(x => x.EventId == 1 || x.EventId == 2)
                    .Any(x => x.Result == true);

                if (userBetCorrect)
                {
                    eventsToBeUpdated.ForEach(x => x.Result = true);
                }
                else
                {
                    eventsToBeUpdated.ForEach(x => x.Result = false);
                }
                _dataContext.SaveChanges();
            }
        }
        public async Task UpdateOvertime(EventBetDto updateEventBetDto)
        {
            int[] eventsIds = { 4, 5 };
            ClearEventResult(eventsIds);

            var listUsersId = _dataContext.Users.Select(x => x.Id).ToListAsync();

            foreach(var userId in await listUsersId)
            {
                var eventsToBeUpdated = await _dataContext.EventBet
                        .Where(x => x.EventId == updateEventBetDto.EventId)
                        .Where(x => x.GameId == updateEventBetDto.GameId)
                        .Where(x => x.AccountId == userId)
                        .ToListAsync();

                var userBetCorrect = _dataContext.EventBet
                    .Where(x => x.GameId == updateEventBetDto.GameId)
                    .Where(x => x.AccountId == userId)
                    .Where(x => x.EventId == 1 || x.EventId == 2)
                    .Any(x => x.Result == true);

                if (userBetCorrect)
                {
                    eventsToBeUpdated.ForEach(x => x.Result = true);
                }
                else
                {
                    eventsToBeUpdated.ForEach(x => x.Result = false);
                }
                _dataContext.SaveChanges();
            }
        }

        public async Task UpdateAtLeast(EventBetDto updateEventBetDto)
        {
            var eventsToBeUpdated = _dataContext.EventBet.Where(x => x.EventId == updateEventBetDto.EventId).Where(x => x.GameId == updateEventBetDto.GameId).ToList();

            foreach (var ev in eventsToBeUpdated)
            {
                if (ev.Value1 >= updateEventBetDto.Value1)
                {
                    ev.Result = true;
                    _dataContext.SaveChanges();
                }
                else
                {
                    ev.Result = false;
                    _dataContext.SaveChanges();
                }
            }
        }

        public async Task UpdateExact(EventBetDto updateEventBetDto)
        {
            var eventsToBeUpdated = _dataContext.EventBet.Where(x => x.EventId == updateEventBetDto.EventId).Where(x => x.GameId == updateEventBetDto.GameId).ToList();

            foreach (var ev in eventsToBeUpdated)
            {
                if (ev.Value1 == updateEventBetDto.Value1 && ev.Value2 == updateEventBetDto.Value2)
                {
                    ev.Result = true;
                    _dataContext.SaveChanges();
                }
                else
                {
                    ev.Result = false;
                    _dataContext.SaveChanges();
                }
            }
        }

        public async Task UpdateScoreUnder(EventBetDto updateEventBetDto)
        {
            var eventsToBeUpdated = _dataContext.EventBet.Where(x => x.EventId == updateEventBetDto.EventId).Where(x => x.GameId == updateEventBetDto.GameId).ToList();

            foreach (var ev in eventsToBeUpdated)
            {
                    ev.Result = true;
                    _dataContext.SaveChanges();
            }
        }

        private void ClearEventResult(int[] eventIds)
        {
            foreach(int i in eventIds)
            {
                var eventsToClear = _dataContext.EventBet.Where(x => x.EventId == i).ToList();
                eventsToClear.ForEach(x => x.Result = false);
                _dataContext.SaveChanges();
            }
        }
    }
}
