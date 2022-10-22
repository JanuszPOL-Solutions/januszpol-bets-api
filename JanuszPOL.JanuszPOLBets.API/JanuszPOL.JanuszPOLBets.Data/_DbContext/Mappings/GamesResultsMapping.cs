using JanuszPOL.JanuszPOLBets.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings;

public class GamesResultsMapping : IEntityTypeConfiguration<GameResult>
{
    public void Configure(EntityTypeBuilder<GameResult> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion<int>().IsRequired();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(
            new GameResult()
            {
                Id = GameResult.Values.Draw,
                Name = "Draw"
            },
            new GameResult()
            {
                Id = GameResult.Values.Team1,
                Name = "Team1"
            },
            new GameResult()
            {
                Id = GameResult.Values.Team2,
                Name = "Team2"
            },
            new GameResult()
            {
                Id = GameResult.Values.Team1ExtraTime,
                Name = "Team1ExtraTime"
            },
            new GameResult()
            {
                Id = GameResult.Values.Team2ExtraTime,
                Name = "Team2ExtraTime"
            },
            new GameResult()
            {
                Id = GameResult.Values.Team1Penalties,
                Name = "Team1Penalties"
            },
            new GameResult()
            {
                Id = GameResult.Values.Team2Penalties,
                Name = "Team2Penalties"
            }
        );
    }
}
