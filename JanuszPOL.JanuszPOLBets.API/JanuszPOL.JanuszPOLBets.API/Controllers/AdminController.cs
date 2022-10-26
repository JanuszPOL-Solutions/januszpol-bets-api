using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Events;
using Microsoft.AspNetCore.Mvc;

namespace JanuszPOL.JanuszPOLBets.API.Controllers
{
    
    public class AdminController : BaseApiController
    {
        private readonly IEventService _eventService;

        public AdminController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPut("match-result")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult>> MatchResult(long gameId, int Team1Score, int Team2Score)
        {
            return await MethodWrapper(async () =>
            {
                return await _eventService.UpdateMatchResults(gameId, Team1Score, Team2Score);
            });
        }

        [HttpPut("bool-event")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult>> BoolEvent(long gameId, long eventId, bool eventHappened)
        {
            return await MethodWrapper(async () =>
            {
                return await _eventService.UpdateBoolEvent(gameId, eventId, eventHappened);
            });
        }
    }
}
