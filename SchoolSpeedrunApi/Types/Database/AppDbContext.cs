using Microsoft.EntityFrameworkCore;

namespace SchoolSpeedrunApi.Types.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<DbUser> Users { get; set; }
    public DbSet<DbLocation> Locations { get; set; }
    public DbSet<DbRun> Runs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure DbRun -> StartLocation
        modelBuilder.Entity<DbRun>()
            .HasOne(r => r.StartLocation)
            .WithMany(l => l.StartRuns)
            .HasForeignKey(r => r.StartLocationId)
            .OnDelete(DeleteBehavior.Restrict); // prevent cascade loops

        // Configure DbRun -> EndLocation
        modelBuilder.Entity<DbRun>()
            .HasOne(r => r.EndLocation)
            .WithMany(l => l.EndRuns)
            .HasForeignKey(r => r.EndLocationId)
            .OnDelete(DeleteBehavior.Restrict); // prevent cascade loops

        // Configure DbRun -> User
        modelBuilder.Entity<DbRun>()
            .HasOne(r => r.User)
            .WithMany(u => u.Runs)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure DbLocation -> User
        modelBuilder.Entity<DbLocation>()
            .HasOne(l => l.User)
            .WithMany(u => u.Locations)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}