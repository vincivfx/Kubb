using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace KubbAdminAPI.Models;

public class User : BaseModel
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
    public byte[]? PasswordHash { get; set; }

    [MaxLength(80)]
    public byte[]? VerificationToken { get; set; }

    [MaxLength(80)]
    public byte[]? RecoverToken { get; set; }

    public DateTime? LastRecoverRequiredTime { get; set; }

    [Required] public required UserStatus Status { get; set; } = 0;

    public User()
    {
        RandomNumberGenerator.Create().GetBytes(Salt = new byte[16]);
    }

    public string SetVerificationToken()
    {
        byte[] tempPassword;
        RandomNumberGenerator.Create().GetBytes(tempPassword = new byte[32]);
        VerificationToken = SHA3_256.Create().ComputeHash(tempPassword);
        return Convert.ToHexString(tempPassword);
    }

    public string SetRecoveryToken()
    {
        byte[] tempPassword;
        RandomNumberGenerator.Create().GetBytes(tempPassword = new byte[32]);
        RecoverToken = SHA3_256.Create().ComputeHash(tempPassword);
        return Convert.ToHexString(tempPassword);
    }

    public bool VerifyVerificationToken(string token)
    {
        if (VerificationToken == null) return false;
        var userVerificationToken = Convert.FromHexString(token);
        var encUserVerificationToken = SHA3_256.Create().ComputeHash(userVerificationToken);
        return VerificationToken.SequenceEqual(encUserVerificationToken);
    }

    public bool VerifyRecoveryToken(string token)
    {
        if (RecoverToken == null) return false;
        var userRecoveryToken = Convert.FromHexString(token);
        var encUserRecoveryToken = SHA3_256.Create().ComputeHash(userRecoveryToken);
        return RecoverToken.SequenceEqual(encUserRecoveryToken);
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
        if (PasswordHash == null) return false;
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