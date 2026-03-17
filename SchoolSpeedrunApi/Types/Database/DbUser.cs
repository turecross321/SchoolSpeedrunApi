using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace SchoolSpeedrunApi.Types.Database;

[PrimaryKey(nameof(CardGuid))]
public class DbUser
{
    [Key]
    [MaxLength(128)]
    public string CardGuid { get; init; } = null!;
    
    [MaxLength(128)]
    public required string Username { get; init; }
    
    [JsonIgnore]
    [MaxLength(64)]
    public string? ProfilePictureHash { get; set; }

    [JsonIgnore]
    // Navigation property for locations
    public List<DbLocation> Locations { get; init; } = new();
        
    [JsonIgnore]
    // Navigation property for runs
    public List<DbRun> Runs { get; init; } = new();
}