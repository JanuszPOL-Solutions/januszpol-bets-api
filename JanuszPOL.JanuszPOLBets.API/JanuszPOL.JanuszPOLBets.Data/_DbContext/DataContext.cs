using JanuszPOL.JanuszPOLBets.Data._DbContext.MappingProfiles;
using JanuszPOL.JanuszPOLBets.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext
{
    public class DataContext : IdentityDbContext<Account, IdentityRole<long>, long>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //TODO: use mapping
            builder.Entity<Account>()
                .Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);
            builder.Entity<Account>().
                Property(x => x.CreatedAt)
                .HasComputedColumnSql("GetUtcDate()");

            builder.ApplyConfiguration(new TeamsMapping());

            base.OnModelCreating(builder);
        }
    }
}