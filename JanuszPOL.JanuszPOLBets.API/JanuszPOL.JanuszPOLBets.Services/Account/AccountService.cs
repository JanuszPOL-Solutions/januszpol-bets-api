using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JanuszPOL.JanuszPOLBets.Data.Identity;
using JanuszPOL.JanuszPOLBets.Repository.Account.Dto;
using JanuszPOL.JanuszPOLBets.Services.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JanuszPOL.JanuszPOLBets.Services.Account
{
    public interface IAccountService
    {
        Task<ServiceResult> RegisterUser(RegisterDto registerDto);
        Task<ServiceResult> RegisterAdmin(RegisterDto registerDto);
        Task<ServiceResult<LoginResult>> Login(LoginDto loginDto);
    }
    
    public class AccountService : IAccountService
    {
        private readonly UserManager<Data.Entities.Account> _userManager;
        private readonly RoleManager<IdentityRole<long>> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountService(UserManager<Data.Entities.Account> userManager,
            RoleManager<IdentityRole<long>> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ServiceResult> RegisterUser(RegisterDto registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.Username);
            if (userExists != null)
            {
                return ServiceResult.WithErrors("User already exists!");
            }

            Data.Entities.Account account = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.Username
            };
            var result = await _userManager.CreateAsync(account, registerDto.Password);
            return !result.Succeeded
                ? ServiceResult.WithErrors("User creation failed! Please check user details and try again.")
                : ServiceResult.WithSuccess();
        }

        public async Task<ServiceResult> RegisterAdmin(RegisterDto registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.Username);
            if (userExists != null)
            {
                return ServiceResult.WithErrors("User already exists!");
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

        public async Task<ServiceResult<LoginResult>> Login(LoginDto loginDto)
        {
            var account = await _userManager.FindByNameAsync(loginDto.UserName);
            if (account != null && await _userManager.CheckPasswordAsync(account, loginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(account);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var isAdmin = userRoles.Any(x => x == UserRoles.Admin);
                var token = GetToken(authClaims);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return ServiceResult<LoginResult>.WithSuccess(new LoginResult(tokenString, account.UserName, isAdmin));
            }

            return ServiceResult<LoginResult>.WithErrors("Username or password incorrect");
        }

        private JwtSecurityToken GetToken(IList<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
