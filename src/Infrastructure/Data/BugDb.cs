using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class BugDb : DbContext 
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Sighting> Sightings => Set<Sighting>();
    public DbSet<Bug> Bugs => Set<Bug>();
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbPath = "/Users/romainrafai/Programming/2025/ASP.NETCORE/HELPDESK_1/HELP_DESK_APP_1/src/Infrastructure/bugdatabase.db";
            Console.WriteLine($"Path is {dbPath}");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }         
    }
}