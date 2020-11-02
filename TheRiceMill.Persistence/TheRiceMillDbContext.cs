using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Domain.Interfaces;

namespace TheRiceMill.Persistence
{
    public class TheRiceMillDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, IdentityUserLogin<string>, RoleClaim, IdentityUserToken<string>>
    {
        public string CurrentUserId { get; set; }

        public TheRiceMillDbContext(DbContextOptions<TheRiceMillDbContext> options) : base(options)
        {

        }

        public DbSet<GatePass> GatePasses { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Charge> Charges { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<StockIn> StockIns { get; set; }
        public DbSet<StockOut> StockOuts { get; set; }
        public DbSet<ProcessedMaterial> ProcessedMaterials { get; set; }
        public DbSet<RateCost> RateCosts { get; set; }
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<VoucherDetail> VoucherDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TheRiceMillDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IBase && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entry in modifiedEntries)
            {
                var entity = (IBase)entry.Entity;
                DateTime now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = CurrentUserId;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = CurrentUserId;
            }
        }
    }
}
