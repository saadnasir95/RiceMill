using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Persistence.Configurations
{

    public class LedgerConfiguration : IEntityTypeConfiguration<Ledger>
    {
        public void Configure(EntityTypeBuilder<Ledger> builder)
        {
            builder.HasOne(p => p.Company)
                .WithMany(b => b.Ledgers)
                .HasForeignKey(p => p.CompanyId);

            builder.HasKey(p => new {p.LedgerType, p.TransactionId});
        }
    }

}