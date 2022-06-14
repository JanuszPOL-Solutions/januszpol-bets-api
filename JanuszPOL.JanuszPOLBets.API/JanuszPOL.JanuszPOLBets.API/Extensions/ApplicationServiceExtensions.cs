using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Services;
using JanuszPOL.JanuszPOLBets.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JanuszPOL.JanuszPOLBets.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContext<DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}