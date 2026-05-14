using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion; // Tillagd för ValueConverter

namespace SchoolSpeedrunApi.Types.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<DbUser> Users { get; set; }
    public DbSet<DbLocation> Locations { get; set; }
    public DbSet<DbRun> Runs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        if (Database.IsSqlite())
        {
            // Converts to ISO 8601-sträng ("O") during storage, och then back to DateTimeOffset
            var dateTimeOffsetConverter = new ValueConverter<DateTimeOffset, string>(
                v => v.ToString("O"),
                v => DateTimeOffset.Parse(v));

            var nullableDateTimeOffsetConverter = new ValueConverter<DateTimeOffset?, string>(
                v => v.HasValue ? v.Value.ToString("O") : null!,
                v => string.IsNullOrEmpty(v) ? null : DateTimeOffset.Parse(v));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTimeOffset))
                    {
                        property.SetValueConverter(dateTimeOffsetConverter);
                    }
                    else if (property.ClrType == typeof(DateTimeOffset?))
                    {
                        property.SetValueConverter(nullableDateTimeOffsetConverter);
                    }
                }
            }
        }

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
