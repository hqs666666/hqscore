using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using IdentityServer4.Dapper.Interfaces;
using IdentityServer4.Dapper.Options;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Dapper.Stores.SqlServer
{
    /// <summary>
    /// 过期授权清理
    /// </summary>
    public class SqlServerPersistedGrants : IPersistedGrants
    {

        #region Ctor

        private readonly ILogger<SqlServerPersistedGrantStore> _logger;
        private readonly DapperStoreOptions _config;

        public SqlServerPersistedGrants(ILogger<SqlServerPersistedGrantStore> logger,
            DapperStoreOptions config)
        {
            _logger = logger;
            _config = config;
        }

        #endregion

        #region Sql

        private const string RemoveExpireGrantSql = @"delete from PersistedGrants where Expiration>@dt";

        #endregion

        /// <summary>
        /// 移除指定时间的过期信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public async Task RemoveExpireToken(DateTime dt)
        {
            using (var connection = new SqlConnection(_config.DbConnectionString))
            {
                var result = await connection.ExecuteAsync(RemoveExpireGrantSql, new { dt });

                _logger.LogDebug($"remove expire grant from database {result}");
            }
        }
    }
}
