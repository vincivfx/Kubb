using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models;

public abstract class BaseModel
{
    
    [Required]
    public DateTime Created { get; init; } = DateTime.UtcNow;
    
    [Required]
    public DateTime Updated { get; set; } = DateTime.UtcNow;
    
    
}