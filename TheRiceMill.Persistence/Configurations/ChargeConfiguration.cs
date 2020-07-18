using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Persistence.Configurations
{
    public class ChargeConfiguration : IEntityTypeConfiguration<Charge>
    {
        public void Configure(EntityTypeBuilder<Charge> builder)
        {
            builder.HasOne(p => p.Sale)
                .WithMany(b => b.Charges)
                .HasForeignKey(p => p.SaleId);
            builder.HasOne(p => p.Purchase)
                .WithMany(b => b.Charges)
                .HasForeignKey(p => p.PurchaseId);

        }
    }
}