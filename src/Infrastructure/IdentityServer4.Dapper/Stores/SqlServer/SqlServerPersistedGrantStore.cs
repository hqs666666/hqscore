using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer4.Dapper.AutoMappers;
using IdentityServer4.Dapper.Options;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Dapper.Stores.SqlServer
{
    /// <summary>
    /// 授权信息存储
    /// </summary>
    public class SqlServerPersistedGrantStore : IPersistedGrantStore
    {
        #region Ctor

        private readonly ILogger<SqlServerPersistedGrantStore> _logger;
        private readonly DapperStoreOptions _config;

        public SqlServerPersistedGrantStore(ILogger<SqlServerPersistedGrantStore> logger,
            DapperStoreOptions config)
        {
            _logger = logger;
            _config = config;
        }

        #endregion

        #region Sql

        private const string GetGrantsBySubjectIdSql = @"select * from PersistedGrants where SubjectId=@subjectId;";

        private const string GetGrantByKeySql = @"select * from PersistedGrants where [Key]=@key;";

        private const string DeleteGrantByClientOrSIdSql =
            @"delete from PersistedGrants where ClientId=@clientId and SubjectId=@subjectId;";

        private const string DeleteGrantByClientOrSIdOrTypeSql =
            @"delete from PersistedGrants where ClientId=@clientId and SubjectId=@subjectId and Type=@type;";

        private const string DeleteGrantByKeySql = @"delete PersistedGrants where [Key]=@key;";

        private const string InsertGrantSql =
            @"insert into PersistedGrants([Key],ClientId,CreationTime,Data,Expiration,SubjectId,Type) values(@Key,@ClientId,@CreationTime,@Data,@Expiration,@SubjectId,@Type);";

        #endregion

        /// <summary>
        /// 根据用户标识获取所有的授权信息
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            using (var connection = new SqlConnection(_config.DbConnectionString))
            {
                var data = (await connection.QueryAsync<Model.PersistedGrant>(GetGrantsBySubjectIdSql,
                    new {subjectId}))?.AsList();

                if (data == null) return null;
                var model = data.Select(x => x.ToModel());

                _logger.LogDebug("{persistedGrantCount} persisted grants found for {subjectId}", data.Count, subjectId);
                return model;
            }
        }

        /// <summary>
        /// 根据key获取授权信息
        /// </summary>
        /// <param name="key">认证信息</param>
        /// <returns></returns>
        public async Task<PersistedGrant> GetAsync(string key)
        {
            using (var connection = new SqlConnection(_config.DbConnectionString))
            {
                var data = await connection.QueryFirstOrDefaultAsync<Model.PersistedGrant>(GetGrantByKeySql, new {key});
                var model = data.ToModel();
                _logger.LogDebug("{persistedGrantKey} found in database: {persistedGrantKeyFound}", key, model != null);
                return model;
            }
        }

        /// <summary>
        /// 根据用户标识和客户端ID移除所有的授权信息
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            using (var connection = new SqlConnection(_config.DbConnectionString))
            {
                var result = await connection.ExecuteAsync(DeleteGrantByClientOrSIdSql, new {clientId, subjectId});
                _logger.LogDebug($"remove {subjectId} {clientId} from database {result}");
            }
        }

        /// <summary>
        /// 移除指定的标识、客户端、类型等授权信息
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="clientId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            using (var connection = new SqlConnection(_config.DbConnectionString))
            {
                var result = await connection.ExecuteAsync(DeleteGrantByClientOrSIdOrTypeSql, new { clientId, subjectId,type });
                _logger.LogDebug($"remove {subjectId} {clientId} {type} from database {result}");
            }
        }

        /// <summary>
        /// 移除指定key的授权信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string key)
        {
            using (var connection = new SqlConnection(_config.DbConnectionString))
            {
                var result = await connection.ExecuteAsync(DeleteGrantByKeySql, new {key});
                _logger.LogDebug($"remove {key} from database {result}");
            }
        }

        /// <summary>
        /// 存储授权信息
        /// </summary>
        /// <param name="grant"></param>
        /// <returns></returns>
        public async Task StoreAsync(PersistedGrant grant)
        {
            using (var connection = new SqlConnection(_config.DbConnectionString))
            {
                //移除防止重复
                await RemoveAsync(grant.Key);

                var result = await connection.ExecuteAsync(InsertGrantSql, grant);

                _logger.LogDebug($"insert grant from database {result}");
            }   
        }
    }
}
