using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer4.Dapper.Stores.MySql
{
    /// <summary>
    /// 资源存储
    /// </summary>
    public class MySqlResourceStore: IResourceStore
    {
        /// <summary>
        /// 根据api名称获取相关信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 根据作用域信息获取接口资源
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 根据scope获取身份资源
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///  获取所有资源实现
        /// </summary>
        /// <returns></returns>
        public Task<Resources> GetAllResourcesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
