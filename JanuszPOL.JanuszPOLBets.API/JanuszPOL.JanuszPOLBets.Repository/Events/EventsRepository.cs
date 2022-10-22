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

        // Add event bet result
        // Get event bets results for game
        // Get event bets results for game and user
    }

    public class EventsRepository : IEventsRepository
    {
        private readonly DataContext _dataContext;

        public EventsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

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

        public async Task<IEnumerable<EventDto>> GetEvents()
        {
            return await Task.FromResult(
                _dataContext
                .Event
                .Include(x => x.EventType)
                .Select(TranslateToEventDto)
                .ToList());
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
    }
}
