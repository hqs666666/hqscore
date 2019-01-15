using System;
using System.Collections.Generic;
using System.Text;
using Hqs.Model.Users;

namespace Hqs.IService.Users
{
    public interface IUserService
    {
        User GetUser(string id);
        User GetUser(string name, string password);
    }
}
