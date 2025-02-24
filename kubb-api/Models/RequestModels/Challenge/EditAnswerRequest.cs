using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Challenge;

public class EditAnswerRequest
{
    [Required]
    public Guid AnswerId { get; set; }
    
    [Required, MaxLength(50)]
    public required string AnswerText { get; set; }
}