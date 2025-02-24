using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Challenge;

public class DeleteAnswerRequest
{
    [Required]
    public Guid AnswerId { get; set; }
}