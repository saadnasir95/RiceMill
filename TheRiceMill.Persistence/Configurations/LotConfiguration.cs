using TheRiceMill.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TheRiceMill.Persistence.Configurations
{
    class LotConfiguration : IEntityTypeConfiguration<Lot>
    {
        public void Configure(EntityTypeBuilder<Lot> builder)
        {
            builder.HasKey(c => new { c.Id, c.Year });
        }
    }
}
