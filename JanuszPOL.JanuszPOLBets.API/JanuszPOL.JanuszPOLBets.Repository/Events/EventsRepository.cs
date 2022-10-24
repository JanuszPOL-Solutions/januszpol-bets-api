using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using JanuszPOL.JanuszPOLBets.Repository.Events.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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
                .Include(x => x.Event)
                .Include(x => x.Event.EventType)
                .Where(x => x.AccountId == accountId && x.GameId == gameId && !x.IsDeleted)
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
            var bet = _dataContext.EventBet.Single(x => x.Id == updateEventBetDto.EventBetId && !x.IsDeleted);
            bet.EventId = updateEventBetDto.EventId; ;
            bet.AccountId = updateEventBetDto.AccountId;
            bet.GameId = updateEventBetDto.GameId;
            bet.Value1 = updateEventBetDto.Value1;
            bet.Value2 = updateEventBetDto.Value2;

            _dataContext.SaveChanges();
        }

        public async Task<IEnumerable<ExistingEventBetDto>> GetBetsForGame(long gameId)
        {
            var bets = _dataContext
                .EventBet
                .Where(x => x.GameId == gameId && !x.IsDeleted);

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
                .Where(x => !x.IsDeleted && addEventBetResultDto
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

        public async Task<IEnumerable<EventBetResultDto>> GetUserBets(long accountId)
        {
            var userCorrectBaseBets = _dataContext
                .EventBet
                .Include(x => x.Event)
                .Where(x => x.AccountId == accountId && !x.IsDeleted);

            return userCorrectBaseBets == null ?
                null :
                userCorrectBaseBets.Select(x => new EventBetResultDto
                {
                    AccountId = x.AccountId,
                    EventId = x.EventId,
                    GameId = x.GameId,
                    Result = x.Result,
                    Value1 = x.Value1,
                    Value2 = x.Value2,
                    BetCost = x.Event.BetCost,
                    WinValue = x.Event.WinValue
                });
        }

        public async Task DeleteEventBet(long betId)
        {
            var bet = _dataContext.EventBet.Single(x => x.Id == betId);

            if (bet.IsDeleted)
            {
                throw new Exception("Event bet already deleted");
            }

            bet.IsDeleted = true;

            await _dataContext.SaveChangesAsync();
        }

        public async Task<ExistingEventBetDto> GetEventBet(long betId)
        {
            var bet = _dataContext.EventBet.Single(x => x.Id == betId && !x.IsDeleted);
            return await Task.FromResult(new ExistingEventBetDto
            {
                EventId = bet.EventId,
                AccountId = bet.AccountId,
                EventBetId = bet.Id,
                GameId = bet.GameId,
                Value1 = bet.Value1,
                Value2 = bet.Value2
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
                Value2 = eventBet.Value2,
                EventName = eventBet.Event.Name,
                EventDescription = eventBet.Event.Description,
                EventType = EventDto.TranslateEventType(eventBet.Event.EventType)
            };
        }
    }
}
