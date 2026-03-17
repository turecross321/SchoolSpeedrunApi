using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SchoolSpeedrunApi.Types.Enums;

namespace SchoolSpeedrunApi.Types.Database;


public class DbLocation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public Position Position { get; set; }

    // Foreign key to user
    [JsonIgnore]
    [Required]
    [MaxLength(128)]
    public string UserCardGuid { get; set; } = null!;

    [ForeignKey(nameof(UserCardGuid))]
    public DbUser User { get; set; } = null!;

    [JsonIgnore]
    public List<DbRun> StartRuns { get; init; } = new();
    [JsonIgnore]
    public List<DbRun> EndRuns { get; init; } = new();
}