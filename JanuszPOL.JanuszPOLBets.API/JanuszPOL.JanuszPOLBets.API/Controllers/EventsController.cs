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
        public async Task<ActionResult<ServiceResult>> AddEventBet([FromBody] EventBetInput eventBetInput)
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
        public async Task<ActionResult<ServiceResult>> AddBaseEventBet([FromBody] BaseEventBetInput baseEventBetInput)
        {
            return await MethodWrapper(async () =>
            {
                return await _eventService.AddBaseEventBet(baseEventBetInput);
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
    }
}
