using JanuszPOL.JanuszPOLBets.Data._DbContext;
using Microsoft.EntityFrameworkCore;

namespace JanuszPOL.JanuszPOLBets.API.Extensions
{
    public static class DataContextExtensions
    {
        public static IServiceCollection RegisterDataContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
