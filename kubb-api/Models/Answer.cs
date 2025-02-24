using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models;

public class Answer : BaseModel
{
    [Key]
    public Guid AnswerId { get; set; } = Guid.NewGuid();
    
    [Required]
    public required int Question { get; set; }
    
    [Required]
    public required Team Team { get; set; }
    
    [Required, MaxLength(50)]
    public required string AnswerText { get; set; }
}