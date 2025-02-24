using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Challenge;

public class SendAnswerRequest
{
    [Required]
    public Guid TeamId { get; set; }
    
    [Required]
    public Guid QuestionId { get; set; }
    
    [Required, MaxLength(50)]
    public required string AnswerText { get; set; }
}