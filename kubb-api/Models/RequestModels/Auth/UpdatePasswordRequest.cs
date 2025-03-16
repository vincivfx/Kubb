using System.ComponentModel.DataAnnotations;

namespace KubbAdminAPI.Models.RequestModels.User;

public class UpdatePasswordRequest
{
    [Required, MaxLength(64)]
    public required string CurrentPassword { get; set; }
    
    [Required, RegularExpression("^.*(?=.{10,})(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[.,;:@#!]).*$")]
    public required string NewPassword { get; set; }
}