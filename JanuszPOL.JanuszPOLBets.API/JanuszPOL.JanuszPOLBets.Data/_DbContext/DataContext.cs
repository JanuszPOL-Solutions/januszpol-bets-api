using JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings;
using JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings;
using JanuszPOL.JanuszPOLBets.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext;

public class DataContext : IdentityDbContext<Account, IdentityRole<long>, long>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GameResult> GamesResults { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AccountsMapping());
        builder.ApplyConfiguration(new TeamsMapping());
        builder.ApplyConfiguration(new GamesMapping());
        builder.ApplyConfiguration(new GamesResultsMapping());

        base.OnModelCreating(builder);
    }
}