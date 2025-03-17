namespace KubbAdminAPI.Models.ResponseModels.Admin;

public class UsersResponse(int count, List<UsersResponse.User> users)
{
    public int Count { get; set; } = count;
    public List<User> Users { get; set; } = users;

    public class User(Models.User user)
    {
        public Guid UserId { get; set; } = user.UserId;
        public string EmailAddress { get; set; } = user.EmailAddress;
        public string Name { get; set; } = user.Name;
        public string Surname { get; set; } = user.Surname;
        public UserStatus Status { get; set; } = user.Status;
    }
}