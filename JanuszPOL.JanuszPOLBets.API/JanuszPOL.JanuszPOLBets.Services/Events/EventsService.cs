using JanuszPOL.JanuszPOLBets.Repository.Events;
using JanuszPOL.JanuszPOLBets.Repository.Events.Dto;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels;
using System.Linq;

namespace JanuszPOL.JanuszPOLBets.Services.Events
{
    public interface IEventService
    {
        Task<ServiceResult<IList<GetEventsResult>>> GetEvents();
        Task<ServiceResult> AddEventBet(EventBetInput eventBetInput);
        Task<ServiceResult> AddBaseEventBet(BaseEventBetInput eventBetInput);
        Task<ServiceResult> AddBaseEventBetResult(BaseEventBetResultInput baseEventBetResultInput);
        Task<ServiceResult<UserScore>> GetUserScore(long accountId);
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

            if (IsBaseBetEventId(eventBetInput.EventId))
            {
                return ServiceResult.WithErrors($"Event bet is based, please use different endpoint");
            }

            var existingBets = await _eventsRepository.GetEventBetsForGameAndUser(eventBetInput.GameId, eventBetInput.AccountId);
            ExistingEventBetDto? editedBet = null;

            if (existingBets != null)
            {
                editedBet = existingBets.SingleOrDefault(x => x.EventId == eventBetInput.EventId);

                var nonBaseBetCount = existingBets
                    .Where(x => !IsBaseBetEventId(x.EventId))
                    .Count();

                if (nonBaseBetCount >= MaxBetsPerAccountAndGame && editedBet == null)
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

        public async Task<ServiceResult> AddBaseEventBet(BaseEventBetInput eventBetInput)
        {
            var betInput = new EventBetInput
            {
                AccountId = eventBetInput.AccountId,
                GameId = eventBetInput.GameId
            };

            long eventId = 0;

            switch(eventBetInput.BetType)
            {
                case BaseBetType.Team1:
                    eventId = _eventsRepository.Team1WinEventId;
                    break;
                case BaseBetType.Team2:
                    eventId = _eventsRepository.Team2WinEventId;
                    break;
                case BaseBetType.Tie:
                    eventId = _eventsRepository.TieEventId;
                    break;

                default:
                    return ServiceResult.WithErrors($"Invalid bet type {eventBetInput.BetType}");
            }

            betInput.EventId = eventId;

            return await AddEventBet(betInput);
        }

        public async Task<ServiceResult> AddBaseEventBetResult(BaseEventBetResultInput baseEventBetResultInput)
        {
            // This method needs to be improved - it needs to check if the result was already placed
            // and if it did then the results needs to be recalculated

            var baseBets = await _eventsRepository.GetBaseEventBetsForGame(baseEventBetResultInput.GameId);

            if (baseBets == null)
            {
                return ServiceResult.WithSuccess();
            }

            long eventTypeId = 0;
            try
            {
                eventTypeId = GetBasicBetIdFromType(baseEventBetResultInput.BetType);
            }
            catch(Exception e)
            {
                ServiceResult.WithErrors($"Error when getting correct bet type, {e}");
            }

            var correctBetIds = baseBets.Where(x => x.EventId == eventTypeId).Select(x => x.EventBetId);
            await _eventsRepository.AddEventBetResult(new AddEventBetResultDto
            {
                EventBetIds = correctBetIds
            });

            return ServiceResult.WithSuccess();
        }

        public async Task<ServiceResult<UserScore>> GetUserScore(long accountId)
        {
            var bets = await _eventsRepository.GetUserBets(accountId);

            var baseBets = bets.Where(x => IsBaseBetEventId(x.EventId));
            var baseBestScore = baseBets?.Count(x => x.Result.GetValueOrDefault()) * 3; // 3 shouldn't be hardcoded here

            var nonBaseBets = bets.Where(x => !IsBaseBetEventId(x.EventId));

            int nonBaseBetWinScore = 0, nonBaseBetCost = 0;

            if (nonBaseBets != null)
            {
                nonBaseBetCost = nonBaseBets.Sum(x => x.BetCost);
                nonBaseBetWinScore = nonBaseBets.Where(x => x.Result.GetValueOrDefault()).Sum(x => x.WinValue);
            }

            return ServiceResult<UserScore>.WithSuccess(new UserScore
            {
                BaseBetsScore = baseBestScore ?? 0,
                NonBaseBetsScore = nonBaseBetWinScore - nonBaseBetCost
            });
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

        private long GetBasicBetIdFromType(BaseBetType baseBetType)
        {
            switch(baseBetType)
            {
                case BaseBetType.Team1:
                    return _eventsRepository.Team1WinEventId;
                case BaseBetType.Team2:
                    return _eventsRepository.Team2WinEventId;
                case BaseBetType.Tie:
                    return _eventsRepository.TieEventId;

                default:
                    throw new Exception($"Invalid bet type {baseBetType}");
            }
        }

        private bool IsBaseBetEventId(long eventId)
        {
            return eventId == _eventsRepository.Team1WinEventId ||
                        eventId == _eventsRepository.Team2WinEventId ||
                        eventId == _eventsRepository.TieEventId;
        }
    }
}
