namespace KubbAdminAPI.Models.ResponseModels.Auth;

public class LoginResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public DateTime TokenExpiry { get; set; }
}