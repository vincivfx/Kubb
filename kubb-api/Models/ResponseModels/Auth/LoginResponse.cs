namespace KubbAdminAPI.Models.ResponseModels.Auth;

public class LoginResponse(Models.User user, Models.Login login, string token)
{
    public Guid LoginId { get; set; } = login.LoginId;
    public string Token { get; set; } = token;
    public string Name { get; set; } = user.Name + " " + user.Surname;
    public DateTime TokenExpiry { get; set; } = login.Expiration;
    public bool MustChangePassword { get; set; } = (user.Status & UserStatus.MustChangePassword) == UserStatus.MustChangePassword;
    public bool IsServerAdministrator { get; set; } = (user.Status & UserStatus.Administrator) == UserStatus.Administrator;
}