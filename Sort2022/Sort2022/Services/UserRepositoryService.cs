using Sort2022.Interfaces;
using Sort2022.Models;

namespace Sort2022.Services
{
    public class UserRepositoryService : IUserRepositoryService
    {
        private List<User> _users = new List<User>();

        public UserRepositoryService()
        {
            _users.Add(new User
            {
                Username = "admin",
                Password = "password1"
            });
        }

        public User GetUser(User user)
        {
            return _users.FirstOrDefault(x => string.Equals(user.Username, x.Username) 
                && string.Equals(user.Password, x.Password));
        }
    }
}
