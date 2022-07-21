using JanuszPOL.JanuszPOLBets.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings
{
    public class GamesMapping : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(x => x.GameId);
            builder.Property(x => x.GameId).UseIdentityColumn().IsRequired();

            builder.Property(x => x.GameDate).IsRequired();

            builder.Property(x => x.Team1Score).IsRequired();

            builder.Property(x => x.Team2Score).IsRequired();


        }
    }
}
