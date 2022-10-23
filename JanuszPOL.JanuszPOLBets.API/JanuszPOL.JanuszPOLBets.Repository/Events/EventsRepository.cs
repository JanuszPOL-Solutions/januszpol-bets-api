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
        long Team1WinEventId { get; }
        long Team2WinEventId { get; }
        long TieEventId { get; }
        // Add event bet result
        // Get event bets results for game
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
    }
}
