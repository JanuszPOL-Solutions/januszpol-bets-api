using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JanuszPOL.JanuszPOLBets.Data.Identity;
using JanuszPOL.JanuszPOLBets.Repository.Account.Dto;
using JanuszPOL.JanuszPOLBets.Services.Account.Extensions;
using JanuszPOL.JanuszPOLBets.Services.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JanuszPOL.JanuszPOLBets.Services.Account;

public interface IAccountService
{
    Task<ServiceResult> RegisterUser(RegisterDto registerDto);
    Task<ServiceResult> RegisterAdmin(RegisterDto registerDto);
    Task<ServiceResult<LoginResult>> Login(LoginDto loginDto);
    Task<ServiceResult<AccountResult>> GetUserData(string username);
}

public class AccountService : IAccountService
{
    private readonly UserManager<Data.Entities.Account> _userManager;
    private readonly RoleManager<IdentityRole<long>> _roleManager;
    private readonly IOptions<AuthConfiguration> _authOptions;
    public AccountService(
        UserManager<Data.Entities.Account> userManager,
        RoleManager<IdentityRole<long>> roleManager,
        IOptions<AuthConfiguration> authOptions)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authOptions = authOptions;
    }

    public async Task<ServiceResult> RegisterUser(RegisterDto registerDto)
    {
        var userExists = await _userManager.FindByNameAsync(registerDto.Username);
        if (userExists != null)
        {
            return ServiceResult.WithErrors("Użytkownik o tym loginie już istnieje");
        }

        Data.Entities.Account account = new()
        {
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerDto.Username
        };
        var result = await _userManager.CreateAsync(account, registerDto.Password);
        if (result.Succeeded)
        {
            return ServiceResult.WithSuccess();
        }
        else
        {
            var errors = result.Errors.Select(x => x.GetErrorMessage()).ToArray();
            return ServiceResult.WithErrors(errors);
        }
    }

    public async Task<ServiceResult> RegisterAdmin(RegisterDto registerDto)
    {
        var userExists = await _userManager.FindByNameAsync(registerDto.Username);
        if (userExists != null)
        {
            return ServiceResult.WithErrors($"Użytkownik z nickiem: {registerDto.Username} już istnieje");
        }

        Data.Entities.Account account = new()
        {
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerDto.Username
        };

        var result = await _userManager.CreateAsync(account, registerDto.Password);
        if (!result.Succeeded)
        {
            return ServiceResult.WithErrors("User creation failed! Please check user details and try again.");
        }

        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await _roleManager.CreateAsync(new IdentityRole<long>(UserRoles.Admin));
        }

        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
        {
            await _roleManager.CreateAsync(new IdentityRole<long>(UserRoles.User));
        }

        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await _userManager.AddToRoleAsync(account, UserRoles.Admin);
        }
        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await _userManager.AddToRoleAsync(account, UserRoles.User);
        }

        return ServiceResult.WithSuccess();
    }

    //i think it can be better
    public async Task<ServiceResult<LoginResult>> Login(LoginDto loginDto)
    {
        var account = await _userManager.FindByNameAsync(loginDto.UserName);
        if (account != null && await _userManager.CheckPasswordAsync(account, loginDto.Password))
        {

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(account);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var isAdmin = userRoles.Any(x => x == UserRoles.Admin);
            var token = GetToken(authClaims);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return ServiceResult<LoginResult>.WithSuccess(new LoginResult
            {
                Token = tokenString,
                Username = account.UserName,
                IsAdmin = isAdmin,
                ExpiresAt = token.ValidTo
            });
        }

        return ServiceResult<LoginResult>.WithErrors("Username or password incorrect");
    }

    public async Task<ServiceResult<AccountResult>> GetUserData(string username)
    {
        var account = await _userManager.FindByNameAsync(username);
        if (account == null)
        {
            return ServiceResult<AccountResult>.WithErrors("Użytkownik nie istnieje");
        }

        var userRoles = await _userManager.GetRolesAsync(account);
        var isAdmin = userRoles.Any(x => x == UserRoles.Admin);

        return ServiceResult<AccountResult>.WithSuccess(new AccountResult
        {
            Username = username,
            IsAdmin = isAdmin
        });
    }

    private JwtSecurityToken GetToken(IList<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Value.Secret));

        var token = new JwtSecurityToken(
            issuer: _authOptions.Value.ValidIssuer,
            audience: _authOptions.Value.ValidAudience,
            expires: DateTime.Now.AddHours(_authOptions.Value.TokenExpirationInHours),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
