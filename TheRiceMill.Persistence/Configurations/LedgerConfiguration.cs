using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Persistence.Configurations
{

    public class LedgerConfiguration : IEntityTypeConfiguration<Ledger>
    {
        public void Configure(EntityTypeBuilder<Ledger> builder)
        {
            builder.HasOne(p => p.Party)
                .WithMany(b => b.Ledgers)
                .HasForeignKey(p => p.PartyId);

            builder.HasKey(p => new { p.LedgerType, p.Id, p.TransactionType });
        }
    }

}