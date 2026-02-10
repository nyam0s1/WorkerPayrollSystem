using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Blazor_Training.Data
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
        public DbSet<Worker> Workers { get; set; }
    }
}
