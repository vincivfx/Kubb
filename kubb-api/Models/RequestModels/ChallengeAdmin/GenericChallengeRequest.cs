using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.ChallengeAdmin;

public class GenericChallengeRequest
{
    [Required]
    public Guid ChallengeId { get; set; }
}