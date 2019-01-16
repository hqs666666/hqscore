using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer4.Dapper.AutoMappers;
using IdentityServer4.Dapper.Options;
using IdentityServer4.Dapper.Stores.SqlServer;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace IdentityServer4.Dapper.Stores.MySql
{
    /// <summary>
    /// 资源存储
    /// </summary>
    public class MySqlResourceStore: IResourceStore
    {
        #region Ctor

        private readonly ILogger<SqlServerPersistedGrantStore> _logger;
        private readonly DapperStoreOptions _config;

        public MySqlResourceStore(ILogger<SqlServerPersistedGrantStore> logger,
            DapperStoreOptions config)
        {
            _logger = logger;
            _config = config;
        }

        #endregion

        #region Sql

        private const string ApiResourceSql = @"select * from ApiResources where name=@Name and Enabled=1;
                                select * from ApiResources t1 inner join ApiScopes t2 on t1.id = t2.ApiResourceId where name=@name and Enable=1;";

        private const string ApiScopeSql =
            @"select distinct t1.* from ApiResources t1 inner join ApiScopes t2 on t1.Id=t2.ApiResourceId where t2.Name in({0}) and Enabled=1;";

        private const string ApiIdentityResourceSql = @"select * from IdentityResources where Enabled=1 and Name in({0})";

        private const string GetScopeByResourceIdSql = @"select * from ApiScopes where ApiResourceId = @id";

        private const string GetAllIdentityResourceSql = "select * from IdentityResources where Enabled = 1;";

        private const string GetAllResourceSql = "select * from ApiResources where Enabled = 1;";

        #endregion

        /// <summary>
        /// 根据api名称获取相关信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            using (var connection = new MySqlConnection(_config.DbConnectionString))
            {
                var multi = await connection.QueryMultipleAsync(ApiResourceSql, new { name });
                var apiResources = multi.Read<Model.ApiResource>().AsList();
                if (apiResources != null && apiResources.Any())
                {
                    var apiResouce = apiResources.First();
                    apiResouce.Scopes = multi.Read<Model.ApiScope>().AsList();
                    _logger.LogDebug($"found {name} api resouce in database");
                    return apiResouce.ToModel();
                }
            }

            _logger.LogDebug($"did not found {name} api resouce in database");
            return new ApiResource(); ;
        }

        /// <summary>
        /// 根据作用域信息获取接口资源
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var apiResourceData = new List<ApiResource>();
            string scopes = string.Empty;
            foreach (var scope in scopeNames)
            {
                scopes += "'" + scope + "',";
            }

            if (scopes == string.Empty)
                return null;
            scopes = scopes.TrimEnd(',');

            using (var connection = new MySqlConnection(_config.DbConnectionString))
            {
                var apir = (await connection.QueryAsync<Model.ApiResource>(string.Format(ApiScopeSql, scopes))).AsList();
                if (apir != null && apir.Any())
                {
                    foreach (var model in apir)
                    {
                        var scopeData = (await connection.QueryAsync<Model.ApiScope>(GetScopeByResourceIdSql, new { id = model.Id }))?.AsList();
                        model.Scopes = scopeData;
                        apiResourceData.Add(model.ToModel());
                    }
                    _logger.LogDebug("Found {scopes} API scopes in database", apiResourceData.SelectMany(x => x.Scopes).Select(x => x.Name));
                }
            }

            return apiResourceData;
        }

        /// <summary>
        /// 根据scope获取身份资源
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var apiResourceData = new List<IdentityResource>();
            string scopes = string.Empty;
            foreach (var scope in scopeNames)
            {
                scopes += "'" + scope + "',";
            }

            if (scopes == string.Empty)
                return null;
            scopes = scopes.TrimEnd(',');

            using (var connection = new MySqlConnection(_config.DbConnectionString))
            {
                //暂不实现 IdentityClaims
                var data = (await connection.QueryAsync<Model.IdentityResource>(string.Format(ApiIdentityResourceSql, scopes)))?.AsList();
                if (data != null && data.Any())
                {
                    foreach (var model in data)
                    {
                        apiResourceData.Add(model.ToModel());
                    }
                }
            }

            return apiResourceData;
        }

        /// <summary>
        ///  获取所有资源实现
        /// </summary>
        /// <returns></returns>
        public async Task<Resources> GetAllResourcesAsync()
        {
            var apiResourceData = new List<ApiResource>();
            var apiIdentityResourceData = new List<IdentityResource>();
            using (var connection = new MySqlConnection(_config.DbConnectionString))
            {
                var data = (await connection.QueryAsync<Model.IdentityResource>(GetAllIdentityResourceSql))?.AsList();
                if (data != null && data.Any())
                {
                    foreach (var item in data)
                    {
                        apiIdentityResourceData.Add(item.ToModel());
                    }
                }

                var apiData = (await connection.QueryAsync<Model.ApiResource>(GetAllResourceSql))?.AsList();
                if (apiData != null && apiData.Any())
                {
                    foreach (var item in apiData)
                    {
                        var scopeData = (await connection.QueryAsync<Model.ApiScope>(GetScopeByResourceIdSql, new { id = item.Id }))?.AsList();
                        item.Scopes = scopeData;
                        apiResourceData.Add(item.ToModel());
                    }
                }
                var model = new Resources(apiIdentityResourceData, apiResourceData);
                return model;
            }
        }
    }
}
