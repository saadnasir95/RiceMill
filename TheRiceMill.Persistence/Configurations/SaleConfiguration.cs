using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Persistence.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasOne(p => p.Party)
                .WithMany(b => b.Sales)
                .HasForeignKey(p => p.PartyId);
            builder.HasOne(p => p.Vehicle)
                .WithMany(b => b.Sales)
                .HasForeignKey(p => p.VehicleId);
            builder.HasOne(p => p.Product)
                .WithMany(b => b.Sales)
                .HasForeignKey(p => p.ProductId);
        }
    }
}