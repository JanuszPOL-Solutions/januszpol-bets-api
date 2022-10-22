using JanuszPOL.JanuszPOLBets.Services;
using JanuszPOL.JanuszPOLBets.Services.Interfaces;
using JanuszPOL.JanuszPOLBets.Services.Teams;

namespace JanuszPOL.JanuszPOLBets.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            //services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITeamsService, TeamsService>();

            return services;
        }
    }
}