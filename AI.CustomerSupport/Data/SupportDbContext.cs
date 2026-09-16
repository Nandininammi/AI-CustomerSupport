using AI.CustomerSupport.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace AI.CustomerSupport.Data
{
    public class SupportDbContext : DbContext
    {
        public SupportDbContext(DbContextOptions<SupportDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
    }
}
