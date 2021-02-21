using Angular_ASPNET_Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Angular_ASPNET_Core.Data
{
    public class CustomersDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<State> States { get; set; }

        public CustomersDbContext (DbContextOptions<CustomersDbContext> options) : base(options) { }
    }
}