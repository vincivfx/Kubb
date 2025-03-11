using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Challenge;

public class GetInfoRequest
{
    [Required]
    public Guid ChallengeId { get; set; }
}