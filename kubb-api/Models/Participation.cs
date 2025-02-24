using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models;

public class Participation : BaseModel
{
    [Key]
    public Guid ParticipationId { get; set; } = Guid.NewGuid();
    
    [Required]
    public required Challenge Challenge { get; set; }
    
    [Required]
    public required User User { get; set; }
    
}