using JanuszPOL.JanuszPOLBets.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace JanuszPOL.JanuszPOLBets.API.Services;

public interface ILoggedUserService
{
    Task<long> GetLoggedUserId(ClaimsPrincipal claimsPrincipal);
}

public class LoggedUserService : ILoggedUserService
{
    private readonly UserManager<Account> _userManager;

    public LoggedUserService(UserManager<Account> userManager)
    {
        _userManager = userManager;
    }

    public async Task<long> GetLoggedUserId(ClaimsPrincipal claimsPrincipal)
    {
        var username = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
        var account = await _userManager.FindByNameAsync(username);
        
        return account.Id;
    }
}

