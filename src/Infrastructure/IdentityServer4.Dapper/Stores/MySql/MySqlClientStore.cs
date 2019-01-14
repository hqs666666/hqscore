using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer4.Dapper.Stores.MySql
{
    /// <summary>
    /// 客户端存储信息
    /// </summary>
    public class MySqlClientStore : IClientStore
    {
        /// <summary>
        /// 根据客户端Id回去客户端信息
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            throw new NotImplementedException();
        }
    }
}
