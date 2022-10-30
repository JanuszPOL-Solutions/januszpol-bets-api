using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Events;
using JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace JanuszPOL.JanuszPOLBets.API.Controllers
{
    [AllowAnonymous]
    public class EventsController : BaseApiController
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
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
                return await _eventService.AddBaseEventBet(baseEventBetInput);
            });
        }

        [HttpPost("ExactScoreBet")]
        [Description("Method for bet bet for score event. That is temporary endpoint")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult<GameEventBetDto>>> AddExactScoreEventBet([FromBody] TwoValuesEventBetInput twoValuesEventBetInput)
        {
            return await MethodWrapper(async () =>
            {
                twoValuesEventBetInput.EventId = 8; //TMP: Hardcoded Game result event
                return await _eventService.Add2ValuesEventBet(twoValuesEventBetInput);
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
        [Description("Method for adding result for a event bet")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult>> AddEventBetResult([FromBody] EventBetInput baseEventBetResultInput)
        {
            return await MethodWrapper(async () =>
            {
                return await _eventService.AddEventBetResult(baseEventBetResultInput);
            });
        }

        [HttpDelete("EventBet")]
        [Description("Method for deleting an event bet")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult>> DeleteEventBet(long betId)
        {
            return await MethodWrapper(async () =>
            {
                return await _eventService.DeleteEventBet(betId);
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
}
