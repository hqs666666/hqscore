using Hqs.IRepository.Users;
using Hqs.IService.Users;
using Hqs.Model.Users;

namespace Hqs.Service.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUser(string id)
        {
            return _userRepository.GetUser(id);
        }

        public User GetUser(string name, string password)
        {
            return _userRepository.GetUser(name,password);
        }
    }
}
