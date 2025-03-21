using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Admin;

public class UpdateUserRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Surname { get; set; }
    [Required]
    public UserStatus Status { get; set; }

    public bool ResetPassword { get; set; } = false;

    public string Password { get; set; } = "";
}