using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Events;
using JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels;
using JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JanuszPOL.JanuszPOLBets.API.Controllers
{
    [AllowAnonymous]
    public class ScoresController : BaseApiController
    {
        private readonly IEventService _eventService;

        public ScoresController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<UserScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult<UserScore>>> GetScore(long accountId)
        {
            return await MethodWrapper(async () =>
            {
                return await _eventService.GetUserScore(accountId);
            });
        }
    }
}
