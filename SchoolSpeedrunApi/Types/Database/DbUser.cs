using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SchoolSpeedrunApi.Types.Enums;

namespace SchoolSpeedrunApi.Types.Database;

[PrimaryKey(nameof(Id))]
public class DbUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    // Acts as the user's password and should therefore never be shown
    [JsonIgnore] 
    [MaxLength(128)] 
    public string CardGuid { get; init; } = null!;
    
    [MaxLength(128)]
    public required string Username { get; init; }
    
    public required DateTimeOffset RegistrationDate { get; init; }
    public required SchoolProgram SchoolProgram { get; init; }
    
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