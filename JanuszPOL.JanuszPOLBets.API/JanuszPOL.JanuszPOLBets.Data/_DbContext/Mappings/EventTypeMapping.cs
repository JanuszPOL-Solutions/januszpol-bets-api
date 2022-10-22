using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings
{
    public class EventTypeMapping : IEntityTypeConfiguration<EventType>
    {
        public const long BooleanId = 1;
        public const long ExactValueId = 2;
        public const long TwoExactValuesId = 3;
        public void Configure(EntityTypeBuilder<EventType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).HasConversion<string>().IsRequired();

            builder.HasData(
                new EventType
                {
                    Id = BooleanId,
                    Type = EventType.RuleType.Boolean
                },
                new EventType
                {
                    Id = ExactValueId,
                    Type = EventType.RuleType.ExactValue
                },
                new EventType
                {
                    Id = TwoExactValuesId,
                    Type = EventType.RuleType.TwoExactValues
                });
        }
    }
}
