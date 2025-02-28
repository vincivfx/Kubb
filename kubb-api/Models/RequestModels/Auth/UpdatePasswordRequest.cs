using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.User;

public class UpdatePasswordRequest
{
    [Required, MaxLength(64)]
    public required string CurrentPassword { get; set; }
    
    [Required, MaxLength(64)]
    public required string NewPassword { get; set; }
}