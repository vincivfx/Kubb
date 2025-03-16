using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Auth;

public class CompleteRecoverPasswordRequest
{
    [Required]
    public required string Token { get; set; }
    [Required, EmailAddress]
    public required string EmailAddress { get; set; }
    [Required, RegularExpression("^.*(?=.{10,})(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[.,;:@#!]).*$")]
    public required string Password { get; set; }
}
