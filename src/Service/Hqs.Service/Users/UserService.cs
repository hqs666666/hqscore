using System.Linq;
using AutoMapper;
using Hqs.Dto.Users;
using Hqs.IService.Users;
using Hqs.Model.Users;
using Microsoft.EntityFrameworkCore;

namespace Hqs.Service.Users
{
    public class UserService : CoreService, IUserService
    {
        private readonly DbSet<User> _userDbSet;
        private readonly DbSet<UserProfile> _userProfileDbSet;

        private readonly IMapper _mapper;
        public UserService(DbContext context, IMapper mapper) : base(context)
        {
            _userDbSet = DataContext.Set<User>();
            _userProfileDbSet = DataContext.Set<UserProfile>();

            _mapper = mapper;
        }

        public User GetUser(string id)
        {
            return _userDbSet.Find(id);
        }

        public UserDto GetUser(string name, string password)
        {
            var user = _userDbSet.FirstOrDefault(p => p.Mobilephone == name && p.Password == password);
            if (user == null)
                return null;

            var userProfile = _userProfileDbSet.Find(user.Id);
            return _mapper.Map<UserDto>(userProfile);
        }
    }
}
