using JanuszPOL.JanuszPOLBets.API.Services;
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
        private readonly ILoggedUserService _loggedUserService;

        public EventsController(IEventService eventService, ILoggedUserService loggedUserService)
        {
            _eventService = eventService;
            _loggedUserService = loggedUserService;
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
        public async Task<ActionResult<ServiceResult>> AddBaseEventBet([FromBody] BaseEventBetInput baseEventBetInput)
        {
            return await MethodWrapper(async () =>
            {
                return await _eventService.AddBaseEventBet(baseEventBetInput);
            });
        }

        [HttpPost("BaseBetResult")]
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


        [HttpPut("admin/update-event-base")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEventBaseBet(UpdateEventBetInput updateEventBetInput)
        {

            if (!TryValidateModel(updateEventBetInput))
            {
                return BadRequest(ModelState);
            }
            
            await _eventService.UpdateEventBaseBet(updateEventBetInput);
            return Ok();
        }

        [HttpPut("admin/update-event-penalties")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEventPenalties(UpdateEventBetInput updateEventBetInput)
        {
            if (!TryValidateModel(updateEventBetInput))
            {
                return BadRequest(ModelState);
            }

            await _eventService.UpdateEventPenalties(updateEventBetInput);
            return Ok();
        }

        [HttpPut("admin/update-event-overtime")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEventOvertime(UpdateEventBetInput updateEventBetInput)
        {

            if (!TryValidateModel(updateEventBetInput))
            {
                return BadRequest(ModelState);
            }
            
            await _eventService.UpdateEventOvertime(updateEventBetInput);
            return Ok();
        }

        [HttpPut("admin/update-at-least")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEventAtLeast(UpdateEventBetInput updateEventBetInput)
        {
            if (!TryValidateModel(updateEventBetInput))
            {
                return BadRequest(ModelState);
            }
            if(updateEventBetInput.Score1 == null)
            {
                return BadRequest("Ale ile wariacie?");
            }

            await _eventService.UpdateEventAtLeast(updateEventBetInput);
            return Ok();
        }

        [HttpPut("admin/update-event-exact")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEventExact(UpdateEventBetInput updateEventBetInput)
        {
            if (!TryValidateModel(updateEventBetInput))
            {
                return BadRequest(ModelState);
            }
            if (updateEventBetInput.Score1 == null || updateEventBetInput.Score2 == null)
            {
                return BadRequest("Ale ile wariacie?");
            }

            await _eventService.UpdateEventExact(updateEventBetInput);
            return Ok();
        }

        [HttpPut("admin/update-event-score-under")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEventScoreUnder(UpdateEventBetInput updateEventBetInput)
        {
            if (!TryValidateModel(updateEventBetInput))
            {
                return BadRequest(ModelState);
            }
            
            await _eventService.UpdateEventScoreUnder(updateEventBetInput);
            return Ok();
        }

        /*[HttpPut("admin/clear-event-result-all")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ClearAllEventResult(UpdateEventBetInput updateEventBetInput)
        {
            if (!TryValidateModel(updateEventBetInput))
            {
                return BadRequest(ModelState);
            }

            await _eventService.UpdateAllEventResult(updateEventBetInput);
            return Ok();
        }*/
    }
}
