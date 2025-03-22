using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.ChallengeAdmin;

public class CreateChallengeRequest
{
    [Required, MaxLength(63)]
    public required string ChallengeName { get; set; }

    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
}