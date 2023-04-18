using CurrencyManagement.Core.Models;
using CurrencyManagement.Data.Configuration;
using CurrencyManagement.Data.Seed;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyDimension> CurrencyDimensions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyDimensionConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencySeed());
            modelBuilder.ApplyConfiguration(new CurrencyDimensionSeed());
        }
    }
}
