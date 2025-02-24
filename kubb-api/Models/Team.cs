using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models;

public class Team : BaseModel
{
    
    [Key]
    public Guid TeamId { get; set; } = Guid.NewGuid();
    
    [Required, MaxLength(63)]
    public required string TeamName { get; set; }
    
    [Required]
    public required User Administrator { get; set; }
    
    [Required]
    public required Challenge Challenge { get; set; }

    [Required] public required TeamType TeamType { get; set; } = TeamType.None;

}

[Flags]
public enum TeamType
{
    None = 0,
    JoinedTeam = 1 << 0,
    GuestTeam = 1 << 1
}