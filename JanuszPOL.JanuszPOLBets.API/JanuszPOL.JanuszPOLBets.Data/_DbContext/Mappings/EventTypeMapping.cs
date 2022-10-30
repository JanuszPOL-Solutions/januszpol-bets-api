using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings
{
    public class EventTypeMapping : IEntityTypeConfiguration<EventType>
    {
        public const long BaseBetId = 1;
        public const long BooleanId = 2;
        public const long ExactValueId = 3;
        public const long TwoExactValuesId = 4;
        public void Configure(EntityTypeBuilder<EventType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion<long>().IsRequired();
            builder.Property(x => x.Type).IsRequired();

            builder.HasData(
                new EventType
                {
                    Id = EventType.RuleType.BaseBet,
                    Type = EventType.RuleType.BaseBet.ToString()
                },
                new EventType
                {
                    Id = EventType.RuleType.Boolean,
                    Type = EventType.RuleType.Boolean.ToString()
                },
                new EventType
                {
                    Id = EventType.RuleType.ExactValue,
                    Type = EventType.RuleType.ExactValue.ToString()
                },
                new EventType
                {
                    Id = EventType.RuleType.TwoExactValues,
                    Type = EventType.RuleType.TwoExactValues.ToString()
                });
        }
    }
}
