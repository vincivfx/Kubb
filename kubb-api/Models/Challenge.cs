using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models;

public class Challenge : BaseModel
{
    [Key]
    public Guid ChallengeId { get; set; } = Guid.NewGuid();
    
    [Required, MaxLength(63)]
    public required string Name {get; set;}
    
    [Required]
    public required User Administrator { get; set; }
    
    [Required]
    public required DateTime StartTime { get; set; }
    
    [Required]
    public required DateTime EndTime { get; set; }
    
    [Required]
    public required List<Question> Questions { get; set; } = [];

    public int MaxTeamPerUser { get; set; } = 0;

    public ChallengeStatus Status { get; set; } = ChallengeStatus.None;

}

[Flags]
public enum ChallengeStatus
{
    None = 0,
    Visible = 1 << 0,
    AllowAnonymousJoin = 1 << 1,
}