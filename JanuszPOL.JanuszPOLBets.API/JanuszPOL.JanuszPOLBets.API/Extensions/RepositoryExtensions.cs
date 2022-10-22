using JanuszPOL.JanuszPOLBets.Repository.Events;
using JanuszPOL.JanuszPOLBets.Repository.Games;
using JanuszPOL.JanuszPOLBets.Repository.Teams;

namespace JanuszPOL.JanuszPOLBets.API.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITeamsRepository, TeamsRepository>();
        services.AddScoped<IGamesRepository, GamesRepository>();
        services.AddScoped<IEventsRepository, EventsRepository>();

        return services;
    }
}
