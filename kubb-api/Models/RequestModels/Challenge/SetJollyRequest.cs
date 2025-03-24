using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Challenge;

public class SetJollyRequest
{
    [Required]
    public Guid TeamId { get; set; }
    [Required]
    public int QuestionId { get; set; }
}