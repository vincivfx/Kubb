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

    public ChallengeStatus Status { get; set; }

    public RunningChallengeStatus RunningStatus { get; set; }

    [Required]
    public required List<string> Questions { get; set; }

}