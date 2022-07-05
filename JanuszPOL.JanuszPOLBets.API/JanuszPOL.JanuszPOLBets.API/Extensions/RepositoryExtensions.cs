using JanuszPOL.JanuszPOLBets.Repository.Teams;

namespace JanuszPOL.JanuszPOLBets.API.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITeamsRepository, TeamsRepository>();

            return services;
        }
    }
}
