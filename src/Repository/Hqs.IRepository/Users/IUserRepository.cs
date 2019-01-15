﻿using System;
using System.Collections.Generic;
using System.Text;
using Hqs.Model.Users;

namespace Hqs.IRepository.Users
{
    public interface IUserRepository
    {
        User GetUser(string id);
        User GetUser(string name, string password);
    }
}
