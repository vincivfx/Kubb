using System.ComponentModel.DataAnnotations;
using KubbAdminAPI.Utils;

namespace KubbAdminAPI.Models.ResponseModels.ChallengeAdmin;

public class UpdateChallengeRequest
{
    [Required]
    public Guid ChallengeId { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public ChallengeStatus Status { get; set; }

    [Required]
    public RunningChallengeStatus RunningStatus { get; set; }

    [Required]
    public required List<string> Questions { get; set; }


    [ValidJSON]
    public required string AlgorithmSettings { get; set; } = "{}";

}