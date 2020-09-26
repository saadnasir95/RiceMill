using TheRiceMill.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TheRiceMill.Persistence.Configurations
{
    class ProcessedMaterialConfiguration : IEntityTypeConfiguration<ProcessedMaterial>
    {
        public void Configure(EntityTypeBuilder<ProcessedMaterial> builder)
        {
            builder.HasOne(c => c.Lot)
                .WithMany(c => c.ProcessedMaterials)
                .HasForeignKey(c => new { c.LotId, c.LotYear, c.CompanyId });
            builder.HasOne(c => c.Product)
                .WithMany(c => c.ProcessedMaterials)
                .HasForeignKey(c => c.ProductId);
        }
    }
}