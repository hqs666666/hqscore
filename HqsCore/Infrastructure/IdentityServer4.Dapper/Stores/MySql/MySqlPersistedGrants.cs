using System;
using System.Threading.Tasks;
using IdentityServer4.Dapper.Interfaces;

namespace IdentityServer4.Dapper.Stores.MySql
{
    /// <summary>
    /// 过期授权清理
    /// </summary>
    public class MySqlPersistedGrants: IPersistedGrants
    {
        /// <summary>
        /// 移除指定时间的过期信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public Task RemoveExpireToken(DateTime dt)
        {
            throw new NotImplementedException();
        }
    }
}
