using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer4.Dapper.Stores.MySql
{
    /// <summary>
    /// 授权信息存储
    /// </summary>
    public class MySqlPersistedGrantStore : IPersistedGrantStore
    {
        /// <summary>
        /// 根据用户标识获取所有的授权信息
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据key获取授权信息
        /// </summary>
        /// <param name="key">认证信息</param>
        /// <returns></returns>
        public Task<PersistedGrant> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据用户标识和客户端ID移除所有的授权信息
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除指定的标识、客户端、类型等授权信息
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="clientId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除指定key的授权信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 存储授权信息
        /// </summary>
        /// <param name="grant"></param>
        /// <returns></returns>
        public Task StoreAsync(PersistedGrant grant)
        {
            throw new NotImplementedException();
        }
    }
}
