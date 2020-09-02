using TheRiceMill.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TheRiceMill.Persistence.Configurations
{
    class StockInConfiguration : IEntityTypeConfiguration<StockIn>
    {
        public void Configure(EntityTypeBuilder<StockIn> builder)
        {
            builder.HasOne(c => c.Lot)
                .WithMany(c => c.StockIns)
                .HasForeignKey(c => new { c.LotId, c.LotYear });
        }
    }
}
