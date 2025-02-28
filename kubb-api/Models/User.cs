using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace KubbAdminAPI.Models;

public class User
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();
    
    [Required, MaxLength(255)]
    public required string EmailAddress { get; set; }
    
    [Required, MaxLength(63)]
    public required string Name { get; set; }
    
    [Required, MaxLength(63)]
    public required string Surname { get; set; }
    
    [Required, MaxLength(24)]
    public byte[] Salt { get; set; }
    
    [Required, MaxLength(80)]
    public byte[] PasswordHash { get; set; }
    
    [MaxLength(80)]
    public byte[] VerificationToken { get; set; }
    
    public DateTime LastRecoverRequiredTime { get; set; }

    [Required] public required UserStatus Status { get; set; } = 0;
    
    public User()
    {
        RandomNumberGenerator.Create().GetBytes(Salt = new byte[16]);
    }

    public string SetVerificationToken()
    {
        byte[] tempPassword;
        RandomNumberGenerator.Create().GetBytes(tempPassword = new byte[16]);
        VerificationToken = SHA3_256.Create().ComputeHash(tempPassword);
        return Convert.ToHexString(tempPassword);
    }

    public string SetTemporaryPassword()
    {
        byte[] tempPassword;
        RandomNumberGenerator.Create().GetBytes(tempPassword = new byte[16]);
        SetPasswordHash(Convert.ToHexString(tempPassword));
        return Convert.ToHexString(tempPassword);
    }

    public void SetPasswordHash(string password)
    {
        PasswordHash = KeyDerivation.Pbkdf2(
            password,
            Salt,
            KeyDerivationPrf.HMACSHA512,
            100000,
            80);

    }

    public bool CheckPassword(string password)
    {
        return KeyDerivation.Pbkdf2(
            password,
            Salt,
            KeyDerivationPrf.HMACSHA512,
            100000,
            80).SequenceEqual(PasswordHash);
        ;
    }
}

[Flags]
public enum UserStatus
{
    None = 0,
    NeedsVerification = 1,
    Active = 2,
    MustChangePassword = 4,
    ChallengeCreator = 8,
    ChallengeJoiner = 16,
    Administrator = 32
}