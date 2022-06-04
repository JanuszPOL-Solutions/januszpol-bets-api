using JanuszPOL.JanuszPOLBets.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext
{
    public class DataContext : IdentityDbContext<Account, IdentityRole<long>, long>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>()
                .Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);
            builder.Entity<Account>().
                Property(x => x.CreatedAt)
                .HasComputedColumnSql("GetUtcDate()");
            base.OnModelCreating(builder);
        }
    }
}