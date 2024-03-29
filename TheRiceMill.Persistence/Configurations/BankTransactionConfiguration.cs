using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Persistence.Configurations
{

    public class BankTransactionConfiguration : IEntityTypeConfiguration<BankTransaction>
    {
        public void Configure(EntityTypeBuilder<BankTransaction> builder)
        {
            builder.HasOne(p => p.Company)
                .WithMany(b => b.BankTransactions)
                .HasForeignKey(p => p.CompanyId);
            builder.HasOne(p => p.BankAccount)
                .WithMany(b => b.BankTransactions)
                .HasForeignKey(p => p.BankAccountId);
            

        }
    }

}