using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models;

public class Question
{
    [Key]
    public Guid QuestionId { get; set; } = Guid.NewGuid();
    
    [Required, MaxLength(200)]
    public required string Description { get; set; }
    
    [Required, MaxLength(2000)]
    public required string Text { get; set; }
    
    public Guid ImageId { get; set; }
    
    [Required, MaxLength(50)]
    public required string AnswerText { get; set; }
    
    [Required]
    public required User Owner { get; set; }
}