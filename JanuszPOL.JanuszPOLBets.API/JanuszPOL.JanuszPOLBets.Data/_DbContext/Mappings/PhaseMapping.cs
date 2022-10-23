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
    internal class PhaseMapping : IEntityTypeConfiguration<Phase>
    {
        public void Configure(EntityTypeBuilder<Phase> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion<int>().IsRequired();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasData(
                new Phase()
                {
                    Id = Phase.Types.Group,
                    Name = "Group"
                },
                new Phase()
                {
                    Id = Phase.Types.Playoffs,
                    Name = "Playoffs"
                }
            );
        }
    }
}
