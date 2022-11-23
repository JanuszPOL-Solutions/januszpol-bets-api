using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using JanuszPOL.JanuszPOLBets.Repository.Events;
using JanuszPOL.JanuszPOLBets.Repository.Events.Dto;
using JanuszPOL.JanuszPOLBets.Repository.Games;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels;
using EventBet = JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels.EventBet;
using EventDto = JanuszPOL.JanuszPOLBets.Repository.Events.Dto.EventDto;
using EventType = JanuszPOL.JanuszPOLBets.Data.Entities.Events.EventType;

namespace JanuszPOL.JanuszPOLBets.Services.Events;

public interface IEventService
{
    Task<ServiceResult<IList<GetEventsResult>>> GetEvents();
    Task<ServiceResult<GameEventBetDto>> AddEventBet(EventBetInput eventBetInput);
    Task<ServiceResult<GameEventBetDto>> Add2ValuesEventBet(TwoValuesEventBetInput eventBetInput);
    Task<ServiceResult<GameEventBetDto>> AddBaseEventBet(BaseEventBetInput eventBetInput);
    Task<ServiceResult<GameEventBetDto>> AddBoolEventBet(BoolBetInput eventBetInput);
    Task<ServiceResult> AddBaseEventBetResult(BaseEventBetResultInput baseEventBetResultInput);
    Task<ServiceResult> AddEventBetResult(EventBetResultInput eventBetInput);
    Task<ServiceResult> DeleteEventBet(long betId, long accountId);
    Task<ServiceResult<UserScore>> GetUserScore(long accountId);
    Task<ServiceResult<IEnumerable<EventBet>>> GetUserBetsForGame(long accountId, long gameId);
    Task<ServiceResult> UpdateMatchResults(long gameId, int Team1Score, int Team2Score);
    Task<ServiceResult> UpdateBoolEvent(long gameId, long eventId, bool eventHappened);

    Task<ServiceResult<RankingDto>> GetFullRanking(RankingFilterInput input);
}

public class EventsService : IEventService
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IGamesRepository _gamesRepository;
    private const int MaxBetsPerAccountAndGame = 2;

    public EventsService(IEventsRepository eventsRepository, IGamesRepository gamesRepository)
    {
        _eventsRepository = eventsRepository;
        _gamesRepository = gamesRepository;
    }

    public async Task<ServiceResult<GameEventBetDto>> AddEventBet(EventBetInput eventBetInput)
    {
        var eventToBet = await _eventsRepository.GetEvent(eventBetInput.EventId);
        if (!IsEventBetValid(eventBetInput, eventToBet, out string message))
        {
            return ServiceResult<GameEventBetDto>.WithErrors(message);
        }

        var existingBets = await _eventsRepository.GetEventBetsForGameAndUser(eventBetInput.GameId, eventBetInput.AccountId);
        ExistingEventBetDto? editedBet = null;

        if (!eventBetInput.IsBaseBet && existingBets != null)
        {
            editedBet = existingBets.SingleOrDefault(x => x.EventId == eventBetInput.EventId);

            var nonBaseBetCount = existingBets
                .Where(x => !IsBaseBetEventId(x.EventId))
                .Count();

            if (nonBaseBetCount >= MaxBetsPerAccountAndGame && editedBet == null)
            {
                return ServiceResult<GameEventBetDto>.WithErrors($"User cannot have more than {MaxBetsPerAccountAndGame} bets per game");
            }
        }

        if (eventBetInput.IsBaseBet && existingBets != null)
        {
            editedBet = existingBets.SingleOrDefault(x => x.EventType == EventType.RuleType.BaseBet);
        }

        if (editedBet == null)
        {
            var addEventBetResult = await _eventsRepository.AddEventBet(new AddEventBetDto
            {
                AccountId = eventBetInput.AccountId,
                EventId = eventBetInput.EventId,
                GameId = eventBetInput.GameId,
                Value1 = eventBetInput.Value1,
                Value2 = eventBetInput.Value2
            });

            return ServiceResult<GameEventBetDto>.WithSuccess(addEventBetResult);
        }

        var editEventBetResult = await _eventsRepository.UpdateEventBet(new ExistingEventBetDto
        {
            EventBetId = editedBet.EventBetId,
            AccountId = editedBet.AccountId,
            EventId = eventBetInput.EventId,
            GameId = eventBetInput.GameId,
            Value1 = eventBetInput.Value1,
            Value2 = eventBetInput.Value2
        });

        return ServiceResult<GameEventBetDto>.WithSuccess(editEventBetResult);
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
                EventType = x.EventType
            }).ToList());
    }

    public async Task<ServiceResult<GameEventBetDto>> Add2ValuesEventBet(TwoValuesEventBetInput eventBetInput)
    {
        if (eventBetInput.Value1 < 0 || eventBetInput.Value2 < 0)
        {
            return ServiceResult<GameEventBetDto>.WithErrors("Scores can't be negative");
        }

        var input = new EventBetInput
        {
            AccountId = eventBetInput.AccountId,
            EventId = eventBetInput.EventId,
            GameId = eventBetInput.GameId,
            Value1 = eventBetInput.Value1,
            Value2 = eventBetInput.Value2
        };

        return await AddEventBet(input);
    }

    public async Task<ServiceResult<GameEventBetDto>> AddBaseEventBet(BaseEventBetInput eventBetInput)
    {
        var betInput = new EventBetInput
        {
            AccountId = eventBetInput.AccountId,
            GameId = eventBetInput.GameId,
            IsBaseBet = true
        };

        long eventId = 0;

        switch (eventBetInput.BetType)
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
                return ServiceResult<GameEventBetDto>.WithErrors($"Invalid bet type {eventBetInput.BetType}");
        }

        betInput.EventId = eventId;

        return await AddEventBet(betInput);
    }

    public async Task<ServiceResult<GameEventBetDto>> AddBoolEventBet(BoolBetInput eventBetInput)
    {
        var input = new EventBetInput
        {
            AccountId = eventBetInput.AccountId,
            EventId = eventBetInput.EventId,
            GameId = eventBetInput.GameId
        };

        return await AddEventBet(input);
    }

    public async Task<ServiceResult> AddBaseEventBetResult(BaseEventBetResultInput baseEventBetResultInput)
    {
        // This method needs to be improved - it needs to check if the result was already placed
        // and if it did then the results needs to be recalculated

        var bets = await _eventsRepository.GetBetsForGame(baseEventBetResultInput.GameId);

        if (bets == null)
        {
            return ServiceResult.WithSuccess();
        }

        long eventTypeId = 0;
        try
        {
            eventTypeId = GetBasicBetIdFromType(baseEventBetResultInput.BetType);
        }
        catch (Exception e)
        {
            return ServiceResult.WithErrors($"Error when getting correct bet type, {e}");
        }

        var correctBaseBetIds = bets.Where(x => x.EventId == eventTypeId).Select(x => x.EventBetId);
        await _eventsRepository.AddEventBetResult(new AddEventBetResultDto
        {
            EventBetIds = correctBaseBetIds
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

    public async Task<ServiceResult<RankingDto>> GetFullRanking(RankingFilterInput input)
    {
        var ranking = await _eventsRepository.GetFullRanking(input.ToDate);

        return ServiceResult<RankingDto>.WithSuccess(ranking);
    }

    public async Task<ServiceResult> AddEventBetResult(EventBetResultInput eventBetInput)
    {
        // Right now we're supporting only couple of even types
        var supportedEventIds = new long[] { 4, 5, 9 };

        if (IsBaseBetEventId(eventBetInput.EventId))
        {
            return ServiceResult.WithErrors("For base bet results use different endpoint");
        }

        if (!supportedEventIds.Any(x => x == eventBetInput.EventId))
        {
            return ServiceResult.WithErrors("Event type result not supported yet");
        }

        var bets = await _eventsRepository.GetBetsForGame(eventBetInput.GameId);

        if (bets == null)
        {
            return ServiceResult.WithSuccess();
        }

        var eventBets = bets.Where(x => x.EventId == eventBetInput.EventId);

        if (eventBets == null)
        {
            return ServiceResult.WithSuccess();
        }

        await _eventsRepository.AddEventBetResult(new AddEventBetResultDto
        {
            EventBetIds = eventBets.Select(x => x.EventBetId)
        });

        return ServiceResult.WithSuccess();
    }

    public async Task<ServiceResult> DeleteEventBet(long betId, long accountId)
    {
        var existingBet = await _eventsRepository.GetEventBet(betId, accountId);

        if (existingBet == null)
        {
            return ServiceResult.WithErrors("Wybrany zakład nie istnieje.");
        }

        if (IsBaseBetEventId(existingBet.EventId))
        {
            return ServiceResult.WithErrors("Zakład rezultatu meczu nie może być usunięty");
        }

        try
        {
            await _eventsRepository.DeleteEventBet(betId);
        }
        catch (Exception e)
        {
            return ServiceResult.WithErrors($"Wystąpił wyjątek w trakcie usuwania zakładu, {e}");
        }

        return ServiceResult.WithSuccess();
    }

    public async Task<ServiceResult<IEnumerable<EventBet>>> GetUserBetsForGame(long accountId, long gameId)
    {
        var bets = await _eventsRepository.GetEventBetsForGameAndUser(gameId, accountId);

        if (bets == null)
        {
            return ServiceResult<IEnumerable<EventBet>>.WithSuccess(new List<EventBet>());
        }

        return ServiceResult<IEnumerable<EventBet>>.WithSuccess(bets.Select(x => new EventBet
        {
            AccountId = accountId,
            GameId = gameId,
            BetId = x.EventBetId,
            Description = x.EventDescription,
            Name = x.EventName,
            EventType = x.EventType,
            Value1 = x.Value1,
            Value2 = x.Value2
        }));
    }

    private bool IsEventBetValid(EventBetInput eventBetInput, EventDto eventToBet, out string message)
    {
        message = string.Empty;

        if (_gamesRepository.GetGameById(eventBetInput.GameId).GetAwaiter().GetResult().Started)
        {
            message = "Cannot bet a game which already started";
            return false;
        }

        if (eventToBet.EventType == Data.Entities.Events.EventType.RuleType.Boolean ||
            eventToBet.EventType == Data.Entities.Events.EventType.RuleType.BaseBet)
        {
            if (eventBetInput.Value1.HasValue || eventBetInput.Value2.HasValue)
            {
                message = "Yes/No bet cannot have values";
                return false;
            }
        }

        if (eventToBet.EventType == EventType.RuleType.ExactValue)
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

        if (eventToBet.EventType == EventType.RuleType.TwoExactValues)
        {
            if (!eventBetInput.Value1.HasValue || !eventBetInput.Value2.HasValue)
            {
                message = "For 2 exact values bet you need to provide both values";
                return false;
            }
        }

        var enoughPoints = _eventsRepository
            .ValidateUserPointsForBet(eventBetInput.AccountId, eventBetInput.EventId, eventBetInput.GameId);
        if (!enoughPoints.Result)
        {
            message = "Niewystarczająca ilość punktów, żeby obstawić :(";
            return false;
        }

        return true;
    }

    private long GetBasicBetIdFromType(BaseBetType baseBetType)
    {
        switch (baseBetType)
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

    public async Task<ServiceResult> UpdateMatchResults(long gameId, int Team1Score, int Team2Score)
    {
        try
        {
            await _gamesRepository.Update(new UpdateGameDto { Id = gameId, Team1Score = Team1Score, Team2Score = Team2Score });
            await _eventsRepository.UpdateBaseBet(gameId, Team1Score, Team2Score);
            await _eventsRepository.UpdateSingleExactBet(gameId, Team1Score, Team2Score);
            await _eventsRepository.UpdateBothExactBet(gameId, Team1Score, Team2Score);
        }
        catch (Exception ex)
        {
            return ServiceResult.WithErrors($"Error when updating match result, {ex}");
        }
        return ServiceResult.WithSuccess();
    }

    public async Task<ServiceResult> UpdateBoolEvent(long gameId, long eventId, bool eventHappened)
    {
        var eventOfType = await _eventsRepository.GetEventById(eventId);
        if (eventOfType.EventTypeId != EventType.RuleType.Boolean)
        {
            return ServiceResult.WithErrors("Only events of type boolean");
        }
        else
        {
            try
            {
                await _eventsRepository.UpdateBoolBet(gameId, eventId, eventHappened);
            }
            catch (Exception ex)
            {
                return ServiceResult.WithErrors($"Error when updating event, {ex}");
            }
            return ServiceResult.WithSuccess();
        }
    }
}
