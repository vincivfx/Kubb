using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Auth;

public class RegisterRequest
{
    [Required, MaxLength(255)]
    public required string EmailAddress { get; set; }
    
    [Required, MaxLength(63)]
    public required string Name { get; set; }
    
    [Required, MaxLength(63)]
    public required string Surname { get; set; }
    
    [Required, MaxLength(63)]
    public required string Password { get; set; }
    
    [Required, MaxLength(8), RegularExpression("^I accept$")]
    public required string TermsAccept { get; set; } // need to be 'I Accept'
    
    public string? TurnstileToken { get; set; }
}