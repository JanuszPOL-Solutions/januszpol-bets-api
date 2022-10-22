using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Games;
using JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels;
using JanuszPOL.JanuszPOLBets.Services.Teams.ServiceModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace JanuszPOL.JanuszPOLBets.API.Controllers
{
    public class GamesController : BaseApiController
    {
        private readonly IGamesService _gamesService;

        public GamesController(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        [HttpGet("all")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ServiceResult<IList<GetGamesResult>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult<IList<GetGamesResult>>>> GetAllGames()
        {


            var result = await _gamesService.GetAll();

            if (result.IsError)
            {
                return BadRequest(result.ErrorsMessage);
            }

            return Ok(result);
        }

        [HttpGet("")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ServiceResult<IList<GetGamesResult>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult<IList<GetGamesResult>>>> GetGames([FromQuery] GetGamesInput input)
        {
            if (!TryValidateModel(input))
            {
                return BadRequest(ModelState);
            }

            var result = await _gamesService.Get(input);

            if (result.IsError)
            {
                return BadRequest(result.ErrorsMessage);
            }

            return Ok(result);
        }
    }
}