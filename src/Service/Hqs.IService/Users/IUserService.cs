using Hqs.Dto.Users;
using Hqs.Model.Users;

namespace Hqs.IService.Users
{
    public interface IUserService
    {
        User GetUser(string id);
        UserDto GetUser(string name, string password);
    }
}
