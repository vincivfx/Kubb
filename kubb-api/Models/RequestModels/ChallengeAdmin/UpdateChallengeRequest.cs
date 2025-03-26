using System.ComponentModel.DataAnnotations;
using KubbAdminAPI.Models.ResponseModels.ChallengeAdmin;
using KubbAdminAPI.Utils;

namespace KubbAdminAPI.Models.RequestModels.ChallengeAdmin;

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

    [Required, Range(1, 50)]
    public int MaxTeamPerUser { get; set; }

    [Required, Range(0, 500)]
    public int BasePoints { get; set; }

}