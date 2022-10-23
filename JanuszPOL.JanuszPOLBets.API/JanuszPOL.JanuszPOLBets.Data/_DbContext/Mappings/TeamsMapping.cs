using JanuszPOL.JanuszPOLBets.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Xml.Linq;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings;

public class TeamsMapping : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn().IsRequired();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.FlagUrl).IsRequired(false).HasMaxLength(256);

        builder.HasData(
            new Team
            {
                Id = 1,
                Name = "MyTeam1",
                FlagUrl = "not used now"
            },
            new Team
            {
                Id = 2,
                Name = "MyTeam2",
                FlagUrl = "not used now"
            });
    }
}
