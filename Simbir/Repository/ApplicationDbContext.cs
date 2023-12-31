﻿using Microsoft.EntityFrameworkCore;
using Simbir.Model;

namespace Simbir.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<BlackList> BlackList { get; set; } = null!;
        public DbSet<Transport> Transport { get; set; } = null!;
        public DbSet<Rent> Rent { get; set; } = null!;
    }
}
