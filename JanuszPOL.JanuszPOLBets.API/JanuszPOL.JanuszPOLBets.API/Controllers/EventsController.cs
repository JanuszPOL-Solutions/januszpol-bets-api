using JanuszPOL.JanuszPOLBets.API.Services;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Events;
using JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace JanuszPOL.JanuszPOLBets.API.Controllers;

public class EventsController : BaseApiController
{
    private readonly IEventService _eventService;
    private readonly ILoggedUserService _loggedUserService;
    public EventsController(
        IEventService eventService,
        ILoggedUserService loggedUserService)
    {
        _eventService = eventService;
        _loggedUserService = loggedUserService;
    }

    [HttpGet("")]
    [Description("Method for getting all available event types for bet")]
    [ProducesResponseType(typeof(ServiceResult<IList<GetEventsResult>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceResult<IList<GetEventsResult>>>> GetAllEvents()
    {
        return await MethodWrapper(async () =>
        {
            return await _eventService.GetEvents();
        });
    }

    [HttpPost("Bet")]
    [Description("Method for adding bet for non-base event bet")]
    [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceResult<GameEventBetDto>>> AddEventBet([FromBody] EventBetInput eventBetInput)
    {
        return await MethodWrapper(async () =>
        {
            var accountId = await _loggedUserService.GetLoggedUserId(User);
            eventBetInput.AccountId = accountId;
            return await _eventService.AddEventBet(eventBetInput);
        });
    }

    [HttpPost("BaseBet")]
    [Description("Method for adding bet for base event bet")]
    [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceResult<GameEventBetDto>>> AddBaseEventBet([FromBody] BaseEventBetInput baseEventBetInput)
    {
        return await MethodWrapper(async () =>
        {
            var accountId = await _loggedUserService.GetLoggedUserId(User);
            baseEventBetInput.AccountId = accountId;
            return await _eventService.AddBaseEventBet(baseEventBetInput);
        });
    }

    [HttpPost("ExactScoreBet")]
    [Description("Method for adding bet for score event. That is temporary endpoint")]
    [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceResult<GameEventBetDto>>> AddExactScoreEventBet([FromBody] TwoValuesEventBetInput twoValuesEventBetInput)
    {
        return await MethodWrapper(async () =>
        {
            var accountId = await _loggedUserService.GetLoggedUserId(User);
            twoValuesEventBetInput.AccountId = accountId;
            twoValuesEventBetInput.EventId = 8; //TMP: Hardcoded Game result event
            return await _eventService.Add2ValuesEventBet(twoValuesEventBetInput);
        });
    }

    [HttpPost("BoolBet")]
    [Description("Add bool bet")]
    [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceResult<GameEventBetDto>>> AddBoolEventBet([FromBody] BoolBetInput boolBetEvent)
    {
        return await MethodWrapper(async () =>
        {
            var accountId = await _loggedUserService.GetLoggedUserId(User);
            boolBetEvent.AccountId = accountId;
            return await _eventService.AddBoolEventBet(boolBetEvent);
        });
    }

    [HttpPost("BaseBetResult")]
    [Description("Method for adding result for a base bet")]
    [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceResult>> AddBaseEventBetResult([FromBody] BaseEventBetResultInput baseEventBetResultInput)
    {
        return await MethodWrapper(async () =>
        {
            return await _eventService.AddBaseEventBetResult(baseEventBetResultInput);
        });
    }

    [HttpPost("EventBetResult")]
    [Description("Method for adding result for a event bet - FOR NOW ONLY COUPLE OF EVENTS SUPPORTED")]
    [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceResult>> AddEventBetResult([FromBody] EventBetResultInput eventBetResult)
    {
        return await MethodWrapper(async () =>
        {
            return await _eventService.AddEventBetResult(eventBetResult);
        });
    }

    [HttpDelete("EventBet/{betId}")]
    [Description("Method for deleting an event bet")]
    [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceResult>> DeleteEventBet(long betId)
    {
        return await MethodWrapper(async () =>
        {
            var accountId = await _loggedUserService.GetLoggedUserId(User);
            return await _eventService.DeleteEventBet(betId, accountId);
        });
    }

    [HttpGet("EventBetsAccountGame")]
    [Description("Method for get best for account and game")]
    [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceResult<IEnumerable<EventBet>>>> GetEventBets(long accountId, long gameId)
    {
        return await MethodWrapper(async () =>
        {
            return await _eventService.GetUserBetsForGame(accountId, gameId);
        });
    }
}
