using JanuszPOL.JanuszPOLBets.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings;

internal class AccountsMapping : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

        builder.Property(x => x.CreatedAt).HasComputedColumnSql("GetUtcDate()");
    }
}
