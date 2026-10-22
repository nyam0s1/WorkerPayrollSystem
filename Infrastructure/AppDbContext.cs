using Blazor_Training.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Blazor_Training.Infrastructure
{
    // Inheriting from DbContext gives us the power to talk to databases
    public class AppDbContext : DbContext
    {
        // The constructor passes configuration (like that connection string you just added) to the base class
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // This line is the magic. It tells EF Core:
        // "Please create a table named 'Workers' that looks exactly like my 'Worker' class"
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payslip> Payslips { get; set; }
        public DbSet<StatutoryConfig> StatutoryConfigs { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Store money at cents precision throughout the payroll model.
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Rates are fractions: decimal(18,2) rounds 0.0275 and 0.015
            // to different rates. Preserve at least six decimal places.
            var statutoryConfig = modelBuilder.Entity<StatutoryConfig>();
            statutoryConfig.Property(config => config.PayeRate).HasPrecision(18, 6);
            statutoryConfig.Property(config => config.ShaRate).HasPrecision(18, 6);
            statutoryConfig.Property(config => config.NssfLowerRate).HasPrecision(18, 6);
            statutoryConfig.Property(config => config.HousingLevy).HasPrecision(18, 6);
        }
    }
}
