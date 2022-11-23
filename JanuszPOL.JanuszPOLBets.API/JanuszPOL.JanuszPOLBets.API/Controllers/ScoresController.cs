using JanuszPOL.JanuszPOLBets.API.Services;
using JanuszPOL.JanuszPOLBets.Repository.Events.Dto;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Events;
using JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JanuszPOL.JanuszPOLBets.API.Controllers
{
    public class ScoresController : BaseApiController
    {
        private readonly IEventService _eventService;
        private readonly ILoggedUserService _loggedUserService;

        public ScoresController(
            IEventService eventService,
            ILoggedUserService loggedUserService)
        {
            _eventService = eventService;
            _loggedUserService = loggedUserService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<UserScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult<UserScore>>> GetScore()
        {
            return await MethodWrapper(async () =>
            {
                var accountId = await _loggedUserService.GetLoggedUserId(User);
                return await _eventService.GetUserScore(accountId);
            });
        }

        [HttpGet("ranking")]
        [ProducesResponseType(typeof(ServiceResult<RankingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult<RankingDto>>> GetRanking([FromQuery] DateTime? rankingDate, [FromQuery] bool futureBets)
        {
            return await MethodWrapper(async () =>
            {
                var toDate = rankingDate;
                if (futureBets)
                {
                    toDate = DateTime.Now.AddYears(1);//TMP simplest solution, should be used the last date of tournament
                }

                return await _eventService.GetFullRanking(new RankingFilterInput { ToDate = toDate });
            });
        }
    }
}
