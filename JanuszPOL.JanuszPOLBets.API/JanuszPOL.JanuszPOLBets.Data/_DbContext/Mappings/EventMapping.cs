using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings
{
    public class EventMapping : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn().IsRequired();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(128);
            builder.Property(x => x.BetCost).IsRequired().HasDefaultValue(1);
            builder.Property(x => x.WinValue).IsRequired().HasDefaultValue(2);
            builder.Property(x => x.EventTypeId).IsRequired();

            builder.HasOne(x => x.EventType).WithMany().HasForeignKey(x => x.EventTypeId);
        }
    }
}
