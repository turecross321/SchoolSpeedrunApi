using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SchoolSpeedrunApi.Types.Database;

public class DbRegistration
{
    public DbRegistration(string cardGuid)
    {
        this.CardGuid = cardGuid;
        this.CreationDate = DateTimeOffset.UtcNow;
        this.ExpiryDate = DateTimeOffset.UtcNow.AddMinutes(5);
        this.Scanned = false;
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
        
    [MaxLength(128)] public string CardGuid { get; init; }
    
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset ExpiryDate { get; init; }
    
    public bool Scanned { get; set; } = false;
}