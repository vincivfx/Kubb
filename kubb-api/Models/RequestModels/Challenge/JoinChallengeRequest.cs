using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Challenge;

public class JoinChallengeRequest
{
    [Required]
    public required Guid ChallengeId { get; set; }
    
}