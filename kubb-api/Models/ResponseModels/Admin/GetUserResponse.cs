using KubbAdminAPI.Models;

namespace KubbAdminAPI.Models.ResponseModels.Admin;

public class GetUserResponse(Models.User user)
{
    public string Name { get; set; } = user.Name;
    public string Surname { get; set; } = user.Surname;
    public string EmailAddress { get; set; } = user.EmailAddress;
    public UserStatus Status { get; set; } = user.Status;
    public DateTime Created { get; set; } = user.Created;
    public DateTime Updated { get; set; } = user.Updated;
}