using JanuszPOL.JanuszPOLBets.Data.Identity;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Events;
using JanuszPOL.JanuszPOLBets.Services.Games;
using JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JanuszPOL.JanuszPOLBets.API.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : BaseApiController
    {
        private readonly IEventService _eventService;
        private readonly IGamesService _gamesService;

        public AdminController(IEventService eventService, IGamesService gamesService)
        {
            _eventService = eventService;
            _gamesService = gamesService;
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

        [HttpPost("add-game")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddGame([FromBody] AddGameInput gameInput)
        {
            if (!TryValidateModel(gameInput))
            {
                return BadRequest(ModelState);
            }
            if (!await _gamesService.AddGame(gameInput))
            {
                return BadRequest("No team found with given ID");
            }

            return Ok();
        }

        [HttpGet("match/{id}")]
        public async Task<ActionResult<ServiceResult<SimpleGameDto>>> GetGame([FromRoute] long id)
        {
            return await MethodWrapper(async () =>
            {
                return await _gamesService.GetSimpleGame(id);
            });
        }
    }
}
