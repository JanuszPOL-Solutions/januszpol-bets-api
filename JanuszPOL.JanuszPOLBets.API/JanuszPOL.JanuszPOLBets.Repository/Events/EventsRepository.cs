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
        Task<IEnumerable<ExistingEventBetDto>> GetBetsForGame(long gameId);
        Task AddEventBetResult(AddEventBetResultDto addEventBetResultDto);
        Task<IEnumerable<EventBetResultDto>> GetUserBets(long accountId);
        Task<ExistingEventBetDto> GetEventBet(long betId);
        Task DeleteEventBet(long betId);
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
            _dataContext.EventBet.Add(new EventBet
            {
                AccountId = addEventBetDto.AccountId,
                EventId = addEventBetDto.EventId,
                GameId = addEventBetDto.GameId,
                Value1 = addEventBetDto.Value1,
                Value2 = addEventBetDto.Value2
            });

            await _dataContext.SaveChangesAsync();
        }

        public async Task<EventDto> GetEvent(long eventId)
        {
            var @event = await _dataContext.Event
                .Select(x => TranslateToEventDto(x))
                .SingleAsync();

            return @event;
        }

        public async Task<IEnumerable<ExistingEventBetDto>> GetEventBetsForGameAndUser(long gameId, long accountId)
        {
            var bets = await _dataContext.EventBet
                .Include(x => x.Event)
                .Include(x => x.Event.EventType)
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

        public async Task UpdateEventBet(ExistingEventBetDto updateEventBetDto)
        {
            var bet = await _dataContext.EventBet
                .SingleAsync(x => x.Id == updateEventBetDto.EventBetId && !x.IsDeleted);

            bet.EventId = updateEventBetDto.EventId; ;
            bet.AccountId = updateEventBetDto.AccountId;
            bet.GameId = updateEventBetDto.GameId;
            bet.Value1 = updateEventBetDto.Value1;
            bet.Value2 = updateEventBetDto.Value2;

            _dataContext.SaveChanges();
        }

        public async Task<IEnumerable<ExistingEventBetDto>> GetBetsForGame(long gameId)
        {
            var bets = await _dataContext.EventBet
                .Where(x => x.GameId == gameId && !x.IsDeleted)
                .Select(x => TranslateToExistingBetDto(x))
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

        public async Task<ExistingEventBetDto> GetEventBet(long betId)
        {
            var bet = await _dataContext
                .EventBet
                .Where(x => x.Id == betId && !x.IsDeleted)
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
                Value2 = eventBet.Value2,
                EventName = eventBet.Event.Name,
                EventDescription = eventBet.Event.Description,
                EventType = EventDto.TranslateEventType(eventBet.Event.EventType)
            };
        }
    }
}
