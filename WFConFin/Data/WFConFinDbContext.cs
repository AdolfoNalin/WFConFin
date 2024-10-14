using Microsoft.EntityFrameworkCore;
using System.Data;
using WFConFin.Models;

namespace WFConFin.Data
{
    public class WFConFinDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Account> Account{ get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Persona> Persona { get; set; }

        public WFConFinDbContext(DbContextOptions<WFConFinDbContext> options) : base(options)
        {
            
        }
    }
}
