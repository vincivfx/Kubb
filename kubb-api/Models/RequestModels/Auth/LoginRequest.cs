using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Auth;

public class LoginRequest
{
    [Required, MaxLength(63)]
    public required string EmailAddress { get; set; }

    [Required, MaxLength(63)]
    public required string Password { get; set; }

    public string? TurnstileToken { get; set; }
}