using JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings;
using JanuszPOL.JanuszPOLBets.Data.Entities;
using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
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
    //public DbSet<EventType> EventTypes { get; set; }
    //public DbSet<Event> Event { get; set; }
    //public DbSet<EventBet> EventBet { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AccountsMapping());
        builder.ApplyConfiguration(new TeamsMapping());
        builder.ApplyConfiguration(new GamesMapping());
        builder.ApplyConfiguration(new GamesResultsMapping());
        //builder.ApplyConfiguration(new EventMapping());
        //builder.ApplyConfiguration(new EventTypeMapping());
        //builder.ApplyConfiguration(new EventBetMapping());

        base.OnModelCreating(builder);
    }
}