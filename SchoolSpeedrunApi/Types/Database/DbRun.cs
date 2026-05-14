using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SchoolSpeedrunApi.Types.Enums;

namespace SchoolSpeedrunApi.Types.Database;

/// <summary>
/// A collection of two Locations with different positions
/// </summary>
public class DbRun
{
    public DbRun() {}

    public DbRun(DbLocation startLocation, DbLocation endLocation, DbUser user)
    {
        StartLocationId = startLocation.Id;
        EndLocationId = endLocation.Id;
        UserId = user.Id;
        FinishDate = endLocation.Date;
        Milliseconds = (endLocation.Date - startLocation.Date).TotalMilliseconds;
        StartPosition = startLocation.Position;
        EndPosition = endLocation.Position;
    }
    
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTimeOffset FinishDate { get; set; }
    
    public double Milliseconds { get; init; }
    public Position StartPosition { get; init; }
    public Position EndPosition { get; init; }

    [JsonIgnore]
    [Required]
    public int StartLocationId { get; set; }
    
    [JsonIgnore]
    [ForeignKey(nameof(StartLocationId))]
    public DbLocation StartLocation { get; set; } = null!;

    [JsonIgnore]
    [Required]
    public int EndLocationId { get; set; }
    
    [JsonIgnore]
    [ForeignKey(nameof(EndLocationId))]
    public DbLocation EndLocation { get; set; } = null!;

    [JsonIgnore]
    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public DbUser User { get; set; } = null!;
}