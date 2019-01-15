using System.Linq;
using Hqs.IRepository;
using Hqs.IRepository.Users;
using Hqs.Model.Users;

namespace Hqs.Repository.SqlServer.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IBaseRepository<User> _useRepository;
    
        public UserRepository( IBaseRepository<User> useRepository) 
        {
            _useRepository = useRepository;
        }

        public User GetUser(string id)
        {
            return _useRepository.GetById(id);
        }

        public User GetUser(string name, string password)
        {
            return _useRepository.TableNoTracking.FirstOrDefault(p => p.Mobilephone == name && p.Password == password);
        }
    }
}
