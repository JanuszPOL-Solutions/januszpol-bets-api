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

    [HttpPut("admin/update-game")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateGame(UpdateGameInput updateInput)
    {
        if (!TryValidateModel(updateInput))
        {
            return BadRequest(ModelState);
        }
        _gamesService.UpdateGame(updateInput);

        return Ok();
    }

    [HttpPost("admin/add-game")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult AddGame([FromBody] AddGameInput gameInput)
    {
        if (!TryValidateModel(gameInput))
        {
            return BadRequest(ModelState);
        }
        if (!_gamesService.AddGame(gameInput))
        {
            return BadRequest("Nie no, tak to nie");
        }

        return Ok();
    }


    [HttpGet("all")]
    [Obsolete("Dont use that, use GetGames")]
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
    public async Task<ActionResult<ServiceResult<SingleGameDto>>> GetGameById(int id)
    {
        var accountId = await _loggedUserService.GetLoggedUserId(User);
        var result = await _gamesService.GetGame(id, accountId);

        if (result.IsError)
        {
            return BadRequest(result.ErrorsMessage);
        }

        return Ok(result);
    }
}