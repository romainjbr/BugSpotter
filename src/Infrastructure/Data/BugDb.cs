using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class BugDb : DbContext 
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Sighting> Sightings => Set<Sighting>();
    public DbSet<Bug> Bugs => Set<Bug>();
 
    public BugDb(DbContextOptions<BugDb> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbPath = "../bugdatabase.db";
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }         
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var bug = modelBuilder.Entity<Bug>();
        var sighting = modelBuilder.Entity<Sighting>();
        var user = modelBuilder.Entity<User>();

        sighting.HasOne(s => s.Bug)
            .WithMany()
            .HasForeignKey(s => s.BugId)
            .OnDelete(DeleteBehavior.Cascade);

        sighting.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}