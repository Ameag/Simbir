using Microsoft.EntityFrameworkCore;
using Simbir.Model;

namespace Simbir.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; } = null!;
    }
}
