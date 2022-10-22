using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Repository.Events.Dto;

namespace JanuszPOL.JanuszPOLBets.Repository.Events
{
    public interface IEventsRepository
    {
        // We're assuming that there won't be a lot of events so we don't need to paginate it
        Task<IEnumerable<EventDto>> GetEvents();

        // Add event bet
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

        public async Task<IEnumerable<EventDto>> GetEvents()
        {
            return await Task.FromResult(_dataContext.Event.Select(x => new EventDto
            {
                BetCost = x.BetCost,
                Description = x.Description,
                EventType = EventDto.TranslateEventType(x.EventType),
                Id = x.Id,
                Name = x.Name,
                WinValue = x.WinValue
            }).ToList());
        }
    }
}
