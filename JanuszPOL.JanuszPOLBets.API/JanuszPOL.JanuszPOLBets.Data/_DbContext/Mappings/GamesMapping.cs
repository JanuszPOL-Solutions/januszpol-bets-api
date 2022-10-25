using JanuszPOL.JanuszPOLBets.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings;

public class GamesMapping : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn().IsRequired();
        builder.Property(x => x.GameDate).IsRequired();

        builder.HasOne(x => x.Team1).WithMany().HasForeignKey(x => x.Team1Id).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Team2).WithMany().HasForeignKey(x => x.Team2Id).OnDelete(DeleteBehavior.Restrict);

        builder.HasData(new Game
        {
            Team1Id = 1,
            Team2Id = 2,
            GameDate = DateTime.UtcNow.AddDays(1),
            Id = 1
        });
    }
}
