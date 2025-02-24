using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Challenge;

public class CreateTeamRequest
{
    [Required, MaxLength(63)]
    public required string Name { get; set; }
    
    [Required]
    public Guid ChallengeId { get; set; }
}