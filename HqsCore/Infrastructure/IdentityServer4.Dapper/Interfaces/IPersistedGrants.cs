using System;
using System.Threading.Tasks;

namespace IdentityServer4.Dapper.Interfaces
{
    /// <summary>
    /// 过期授权清理
    /// </summary>
    public interface IPersistedGrants
    {
        /// <summary>
        /// 移除指定时间的过期信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Task RemoveExpireToken(DateTime dt);
    }
}
