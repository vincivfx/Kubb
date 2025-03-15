using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Auth;

public class RecoverPasswordRequest
{
    [Required, MaxLength(255), EmailAddress]
    public required string EmailAddress { get; set; }
    
    public string? TurnstileToken { get; set; }
}