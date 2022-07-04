using BillingAPI.API.Account;
using BillingAPI.API.Balance;
using BillingAPI.API.Gateway;
using BillingAPI.API.Order;
using BillingAPI.API.Payments;
using BillingAPI.API.User;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base()
        {
        }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GatewayEntity>()
                .HasIndex(g => g.No)
                .IsUnique();

            builder.Entity<OrderEntity>()
                .HasIndex(o => o.No)
                .IsUnique();

            builder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<AccountEntity>()
                .HasIndex(a => a.Username)
                .IsUnique();
        }

        public virtual DbSet<BalanceEntity> Balances { get; set; }
        public virtual DbSet<GatewayEntity> Gateways { get; set; }
        public virtual DbSet<OrderEntity> Orders { get; set; }
        public virtual DbSet<PaymentEntity> Payments { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<AccountEntity> Accounts { get; set; }
    }
}
