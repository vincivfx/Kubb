using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Auth;

public class CompleteRegistrationRequest
{
    [Required, MaxLength(255)]
    public required string EmailAddress { get; set; }
    
    [Required, MaxLength(255)]
    public required string RegistrationToken { get; set; }
}