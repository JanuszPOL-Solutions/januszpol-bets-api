using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings
{
    public class EventTypeMapping : IEntityTypeConfiguration<EventType>
    {
        public void Configure(EntityTypeBuilder<EventType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).HasConversion<string>().IsRequired();

            builder.HasData(
                new EventType
                {
                    Id = 1,
                    Type = EventType.RuleType.Boolean
                },
                new EventType
                {
                    Id = 2,
                    Type = EventType.RuleType.ExactValue
                });
        }
    }
}
