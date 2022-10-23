using JanuszPOL.JanuszPOLBets.Repository.Events;
using JanuszPOL.JanuszPOLBets.Repository.Events.Dto;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels;

namespace JanuszPOL.JanuszPOLBets.Services.Events
{
    public interface IEventService
    {
        Task<ServiceResult<IList<GetEventsResult>>> GetEvents();
        Task<ServiceResult> AddEventBet(EventBetInput eventBetInput);
    }

    public class EventsService : IEventService
    {
        private readonly IEventsRepository _eventsRepository;
        private const int MaxBetsPerAccountAndGame = 2;

        public EventsService(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public async Task<ServiceResult> AddEventBet(EventBetInput eventBetInput)
        {
            var eventToBet = await _eventsRepository.GetEvent(eventBetInput.EventId);
            if (!IsEventBetValid(eventBetInput, eventToBet, out string message))
            {
                return ServiceResult.WithErrors($"Error when validating the bet, {message}");
            }

            var existingBets = await _eventsRepository.GetEventBetsForGameAndUser(eventBetInput.GameId, eventBetInput.AccountId);
            ExistingEventBetDto? editedBet = null;

            if (existingBets != null)
            {
                editedBet = existingBets.SingleOrDefault(x => x.EventId == eventBetInput.EventId);

                // This doesn't consider the distinction between base bets and others now - to be done
                if (existingBets.Count() >= MaxBetsPerAccountAndGame && editedBet == null)
                {
                    return ServiceResult.WithErrors($"User cannot have more than {MaxBetsPerAccountAndGame} bets per game");
                }
            }

            if (editedBet == null)
            {
                await _eventsRepository.AddEventBet(new AddEventBetDto
                {
                    AccountId = eventBetInput.AccountId,
                    EventId = eventBetInput.EventId,
                    GameId = eventBetInput.GameId,
                    Value1 = eventBetInput.Value1,
                    Value2 = eventBetInput.Value2
                });

                return ServiceResult.WithSuccess();
            }

            await _eventsRepository.UpdateEventBet(new ExistingEventBetDto
            {
                EventBetId = editedBet.EventBetId,
                AccountId = editedBet.AccountId,
                EventId = editedBet.EventId,
                GameId = editedBet.GameId,
                Value1 = eventBetInput.Value1,
                Value2 = eventBetInput.Value2
            });

            return ServiceResult.WithSuccess();
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
                    WinValue = x.WinValue,
                    EventType = GetEventsResult.TranslateEventType(x.EventType)
                }).ToList());
        }

        private bool IsEventBetValid(EventBetInput eventBetInput, EventDto eventToBet, out string message)
        {
            message = String.Empty;

            if (eventToBet.EventType == Repository.Events.Dto.EventType.Boolean || 
                eventToBet.EventType == Repository.Events.Dto.EventType.BaseBet)
            {
                if (eventBetInput.Value1.HasValue || eventBetInput.Value2.HasValue)
                {
                    message = "Yes/No bet cannot have values";
                    return false;
                }
            }

            if (eventToBet.EventType == Repository.Events.Dto.EventType.ExactValue)
            {
                if (!eventBetInput.Value1.HasValue)
                {
                    message = "For exact bet type you need to provide value";
                    return false;
                }

                if (eventBetInput.Value2.HasValue)
                {
                    message = "For single exact bet type you need to provide only first value";
                    return false;
                }
            }

            if (eventToBet.EventType == Repository.Events.Dto.EventType.TwoExactValues)
            {
                if (!eventBetInput.Value1.HasValue || !eventBetInput.Value2.HasValue)
                {
                    message = "For 2 exact values bet you need to provide both values";
                    return false;
                }
            }

            return true;
        }
    }
}
