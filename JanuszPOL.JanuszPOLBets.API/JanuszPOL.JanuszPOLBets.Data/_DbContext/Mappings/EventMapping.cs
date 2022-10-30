using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings
{
    public class EventMapping : IEntityTypeConfiguration<Event>
    {
        private const int DefaultBetCost = 1;
        private const int DefaultWinValue = 2;

        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn().IsRequired().ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(128);
            builder.Property(x => x.BetCost).IsRequired().HasDefaultValue(DefaultBetCost);
            builder.Property(x => x.WinValue).IsRequired().HasDefaultValue(DefaultWinValue);
            builder.Property(x => x.EventTypeId).IsRequired();

            builder.HasOne(x => x.EventType).WithMany().HasForeignKey(x => x.EventTypeId);

            var id = 1;

            var eventsCollection = new List<Event>
            {
                new Event
                {
                    Id = id++,
                    Name = "Wygrana pierwszej drużyny",
                    Description = "",
                    EventTypeId = (EventType.RuleType)EventTypeMapping.BaseBetId,
                    BetCost = 0,
                    WinValue = 3
                },
                new Event
                {
                    Id = id++,
                    Name = "Wygrana drugiej drużyny",
                    Description = "",
                    EventTypeId = (EventType.RuleType)EventTypeMapping.BaseBetId,
                    BetCost = 0,
                    WinValue = 3
                },
                new Event
                {
                    Id = id++,
                    Name = "Remis",
                    Description = "",
                    EventTypeId = (EventType.RuleType)EventTypeMapping.BaseBetId,
                    BetCost = 0,
                    WinValue = 3
                },
                new Event
                {
                    Id = id++,
                    Name = "Wygrana w dogrywce",
                    Description = "Zadecyduj czy mecz zakończy się w doliczonym czasie gry",
                    EventTypeId = (EventType.RuleType)EventTypeMapping.BooleanId
                },
                new Event
                {
                    Id = id++,
                    Name = "Wygrana w karnych",
                    Description = "Zadecyduj czy mecz zakończy się doperio po rzutach karnych",
                    EventTypeId = (EventType.RuleType)EventTypeMapping.BooleanId
                },
                new Event
                {
                    Id = id++,
                    Name = "Ilość żółtych kartek (>=)",
                    Description = "Zadecyduj ile co najmniej zostanie pokazanych zółtych kartek",
                    EventTypeId = (EventType.RuleType)EventTypeMapping.BooleanId
                },
                new Event
                {
                    Id = id++,
                    Name = "Ilość bramek",
                    Description = "Zadecyduj ile co najmniej padnie bramek",
                    EventTypeId = (EventType.RuleType)EventTypeMapping.ExactValueId
                },
                new Event
                {
                    Id = id++,
                    Name = "Dokładny wynik",
                    Description = "Wytypuj konkretny wynik meczu",
                    EventTypeId = (EventType.RuleType)EventTypeMapping.TwoExactValuesId
                }
            };

            var goalsBeforeValues = new int[] { 10 };

            foreach (var minute in goalsBeforeValues)
            {
                eventsCollection.Add(new Event
                {
                    Id = id++,
                    Name = $"Bramka do {minute} minuty",
                    Description = $"Zadecyduj czy bramka padnie do {minute} minuty",
                    EventTypeId = (EventType.RuleType)EventTypeMapping.BooleanId
                });
            }

            builder.HasData(eventsCollection);
        }
    }
}
