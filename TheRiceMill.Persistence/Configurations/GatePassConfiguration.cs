using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Persistence.Configurations
{
    public class GatePassConfiguration : IEntityTypeConfiguration<GatePass>
    {
        public void Configure(EntityTypeBuilder<GatePass> builder)
        {
            builder.HasOne(p => p.Company)
                .WithMany(b => b.GatePasses)
                .HasForeignKey(p => p.CompanyId);
            builder.HasOne(p => p.Vehicle)
                .WithMany(b => b.GatePasses)
                .HasForeignKey(p => p.VehicleId);
            builder.HasOne(p => p.Product)
                .WithMany(b => b.GatePasses)
                .HasForeignKey(p => p.ProductId);
            builder.HasOne(s => s.Sale)
                .WithMany(g => g.GatePasses)
                .HasForeignKey(s => s.SaleId);
            builder.HasOne(p => p.Purhcase)
                .WithMany(g => g.GatePasses)
                .HasForeignKey(s => s.PurchaseId);
        }
    }
}