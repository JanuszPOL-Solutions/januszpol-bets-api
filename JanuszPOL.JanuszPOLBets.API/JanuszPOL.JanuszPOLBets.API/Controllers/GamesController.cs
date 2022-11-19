using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using JanuszPOL.JanuszPOLBets.API.Services;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Games;
using JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace JanuszPOL.JanuszPOLBets.API.Controllers;

public class GamesController : BaseApiController

{
    private readonly IGamesService _gamesService;
    private readonly ILoggedUserService _loggedUserService;

    public GamesController(IGamesService gamesService, ILoggedUserService loggedUserService)
    {
        _gamesService = gamesService;
        _loggedUserService = loggedUserService;
    }

    [HttpGet("")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServiceResult<IList<GetGamesResult>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceResult<IList<GetGameResultDto>>>> GetGames([FromQuery] GetGamesInput input)
    {
        if (!TryValidateModel(input))
        {
            return BadRequest(ModelState);
        }

        input.AccountId = await _loggedUserService.GetLoggedUserId(User);
        var result = await _gamesService.Get(input);

        if (result.IsError)
        {
            return BadRequest(result.ErrorsMessage);
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceResult<SingleGameWithEventsDto>>> GetGameById(int id)
    {
        var accountId = await _loggedUserService.GetLoggedUserId(User);
        var result = await _gamesService.GetGameWithEvents(id, accountId);

        if (result.IsError)
        {
            return BadRequest(result.ErrorsMessage);
        }

        return Ok(result);
    }
}