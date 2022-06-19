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

        public virtual DbSet<Balance> Balances { get; set; }
        public virtual DbSet<Gateway> Gateways { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
