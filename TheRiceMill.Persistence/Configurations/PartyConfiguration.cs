using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Persistence.Configurations
{
    public class PartyConfiguration : IEntityTypeConfiguration<Party>
    {
        public void Configure(EntityTypeBuilder<Party> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.NormalizedName).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Address).HasMaxLength(255);
            builder.Property(p => p.PhoneNumber).HasMaxLength(20);
        }
    }
}