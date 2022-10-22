using JanuszPOL.JanuszPOLBets.Repository.Events;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels;
using JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels;

namespace JanuszPOL.JanuszPOLBets.Services.Events
{
    public interface IEventService
    {
        Task<ServiceResult<IList<GetEventsResult>>> GetEvents();
    }

    public class EventsService : IEventService
    {
        private readonly IEventsRepository _eventsRepository;

        public EventsService(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public async Task<ServiceResult<IList<GetEventsResult>>> GetEvents()
        {
            var events = await _eventsRepository.GetEvents();

            return ServiceResult<IList<GetEventsResult>>
                .WithSuccess(events.Select(x => new GetEventsResult
                {
                    BetCost = x.BetCost,
                    Description = x.Description,
                    Name = x.Name,
                    Id = x.Id,
                    WinValue = x.WinValue
                }).ToList());
        }
    }
}
