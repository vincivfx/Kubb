namespace KubbAdminAPI.Models.ResponseModels.Auth;

public class LoginResponse
{
    public Guid LoginId {get;set;}
    public string? Token { get; set; }
    public string Name {get;set;}
    public DateTime TokenExpiry { get; set; }
}