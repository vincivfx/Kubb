using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.Admin;

public class CreateUserRequest
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Surname { get; set; }
    [Required, EmailAddress]
    public required string EmailAddress { get; set; }

    public string? Password { get; set; }


    [Required]
    public UserStatus Status { get; set; }

    [Required]
    public bool SendVerificationEmail { get; set; }
    [Required]
    public bool SendRandomPassword {get;set;}
}