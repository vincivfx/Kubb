using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.ResponseModels.ChallengeAdmin;

public class UpdateChallengeRequest
{
    [Required]
    public Guid ChallengeId { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    public DateTime? StartTime { get; set; }
    
    public DateTime? EndTime { get; set; }
    
}