using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Events;
using JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult>> AddBaseEventBet([FromBody] BaseEventBetInput eventBetInput)
        {
            return await MethodWrapper(async () =>
            {
                return await _eventService.AddEventBet(eventBetInput);
            });
        }
    }
}
