using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Challenge;

public class DeleteTeamRequest
{
    [Required]
    public Guid TeamId { get; set; }
}