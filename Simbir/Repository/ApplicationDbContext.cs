using Microsoft.EntityFrameworkCore;
using Simbir.Model;
//План б
//Database.EnsureDeleted();   
//Database.EnsureCreated();
namespace Simbir.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<BlackList> BlackList { get; set; } = null!;
        public DbSet<Transport> Transport { get; set; } = null!;
    }
}
