using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Teams;
using JanuszPOL.JanuszPOLBets.Services.Teams.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace JanuszPOL.JanuszPOLBets.API.Controllers
{
    public class TeamsController : BaseApiController
    {
        private readonly ITeamsService _teamsService;

        public TeamsController(ITeamsService teamsService)
        {
            _teamsService = teamsService;
        }

        [HttpGet("")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ServiceResult<IList<GetTeamsResult>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<IList<GetTeamsResult>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult<IList<GetTeamsResult>>>> Teams([FromQuery] GetTeamsInput input)
        {
            //TODO: add validation
            var result = await _teamsService.Get(input);

            return Ok(result);
        }
    }
}
