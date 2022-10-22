using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using JanuszPOL.JanuszPOLBets.Repository.Account.Dto;
using JanuszPOL.JanuszPOLBets.Services.Account;
using JanuszPOL.JanuszPOLBets.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JanuszPOL.JanuszPOLBets.API.Controllers;

[AllowAnonymous]
public class AccountsController : BaseApiController
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _accountService.RegisterUser(registerDto);

        if (result.IsError)
        {
            return BadRequest(result.ErrorsMessage);
        }

        return Ok(result);
    }

    [HttpPost("register-admin")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto registerDto)
    {
        var result = await _accountService.RegisterAdmin(registerDto);

        if (result.IsError)
        {
            return BadRequest(result.ErrorsMessage);
        }

        return Ok(result);
    }
    [HttpPost("login")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _accountService.Login(loginDto);

        if (result.IsError)
        {
            return BadRequest(result.ErrorsMessage);
        }

        var token = new JwtSecurityTokenHandler().WriteToken(result.Result);
        return Ok(token);
    }

}