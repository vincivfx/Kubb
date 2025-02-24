namespace KubbAdminAPI.Models.ResponseModels.User;

public class GetProfileResponse
{
    public required string EmailAddress { get; set; }
    
    public required string Name { get; set; }
    
    public required string Surname { get; set; }
    
    public UserStatus Status { get; set; }
}