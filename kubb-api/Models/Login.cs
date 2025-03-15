using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace KubbAdminAPI.Models;

public class Login : BaseModel
{
    [Key]
    public Guid LoginId { get; set; }
    
    [Required]
    public required User User { get; set; }
    
    [Required]
    public DateTime Expiration { get; set; } = DateTime.UtcNow.AddMinutes(20);
    
    [Required, MaxLength(64)]
    public byte[] TokenHash { get; set; }

    public string SetToken()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(32);
        this.TokenHash = SHA3_512.HashData(tokenBytes);
        return Convert.ToBase64String(tokenBytes);
    }

    public bool ValidateToken(string token)
    {
        var tokenBytes = SHA3_512.HashData(Convert.FromBase64String(token));
        return this.TokenHash.SequenceEqual(tokenBytes);
    }
}