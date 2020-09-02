using TheRiceMill.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TheRiceMill.Persistence.Configurations
{
    class RateCostConfiguration : IEntityTypeConfiguration<RateCost>
    {
        public void Configure(EntityTypeBuilder<RateCost> builder)
        {
            builder.HasOne(c => c.Lot)
                .WithMany(c => c.RateCosts)
                .HasForeignKey(c => new { c.LotId, c.LotYear });
        }
    }
}