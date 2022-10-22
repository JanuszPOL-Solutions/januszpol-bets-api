using JanuszPOL.JanuszPOLBets.Services.Events;
using JanuszPOL.JanuszPOLBets.Services.Games;
using JanuszPOL.JanuszPOLBets.Services.Teams;

namespace JanuszPOL.JanuszPOLBets.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        //services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ITeamsService, TeamsService>();
        services.AddScoped<IGamesService, GamesService>();
        services.AddScoped<IEventService, EventsService>();

        return services;
    }
}