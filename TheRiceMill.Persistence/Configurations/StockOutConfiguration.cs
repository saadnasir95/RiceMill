using TheRiceMill.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TheRiceMill.Persistence.Configurations
{
    class StockOutConfiguration : IEntityTypeConfiguration<StockOut>
    {
        public void Configure(EntityTypeBuilder<StockOut> builder)
        {
            builder.HasOne(c => c.Lot)
                .WithMany(c => c.StockOuts)
                .HasForeignKey(c => new { c.LotId, c.LotYear, c.CompanyId });
            builder.HasOne(c => c.Product)
                .WithMany(c => c.StockOuts)
                .HasForeignKey(c => c.ProductId);
        }
    }
}