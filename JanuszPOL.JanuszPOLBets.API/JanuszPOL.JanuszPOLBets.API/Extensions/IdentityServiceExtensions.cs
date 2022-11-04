using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Data.Entities;
using Microsoft.AspNetCore.Identity;
using JanuszPOL.JanuszPOLBets.Services.Account;

namespace JanuszPOL.JanuszPOLBets.API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        // For Identity
        services.AddIdentity<Account, IdentityRole<long>>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        const string authSection = "JWT";
        services.Configure<AuthConfiguration>(config.GetSection(authSection));

        // Adding Authentication
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = config[$"{authSection}:ValidAudience"],
                    ValidIssuer = config[$"{authSection}:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[$"{authSection}:Secret"]))
                };
            });

        return services;
    }
}