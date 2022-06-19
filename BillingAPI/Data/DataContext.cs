using BillingAPI.Entities;
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
            builder.Entity<Gateway>()
                .HasIndex(g => g.No)
                .IsUnique();

            builder.Entity<Order>()
                .HasIndex(o => o.No)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }


        public virtual DbSet<Balance> Balances { get; set; }
        public virtual DbSet<Gateway> Gateways { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
