using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings
{
    public class EventBetMapping : IEntityTypeConfiguration<EventBet>
    {
        void IEntityTypeConfiguration<EventBet>.Configure(EntityTypeBuilder<EventBet> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AccountId).IsRequired();
            builder.Property(x => x.GameId).IsRequired();
            builder.Property(x => x.EventId).IsRequired();

            builder.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
            builder.HasOne(x => x.Event).WithMany(x => x.Bets).HasForeignKey(x => x.EventId);
            builder.HasOne(x => x.Game).WithMany(x => x.EventBets).HasForeignKey(x => x.GameId);
        }
    }
}
