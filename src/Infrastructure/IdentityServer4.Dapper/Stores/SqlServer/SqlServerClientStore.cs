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
    /// 客户端存储信息
    /// </summary>
    public class SqlServerClientStore : IClientStore
    {
        #region Ctor

        private readonly ILogger<SqlServerClientStore> _logger;
        private readonly DapperStoreOptions _dapperConfig;

        public SqlServerClientStore(ILogger<SqlServerClientStore> logger,
            DapperStoreOptions dapperConfig)
        {
            _logger = logger;
            _dapperConfig = dapperConfig;
        }

        #endregion

        #region Sql

        private const string ClientSql = @"select * from Clients where ClientId = @client and Enabled = 1;";

        private const string OtherClientInfoSql = @"select t2.* from Clients t1 inner join ClientGrantTypes t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
                                select t2.* from Clients t1 inner join ClientRedirectUris t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
                                select t2.* from Clients t1 inner join ClientScopes t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
                                select t2.* from Clients t1 inner join ClientSecrets t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;";
        #endregion

        /// <summary>
        /// 根据客户端Id回去客户端信息
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            using (var connection = new SqlConnection(_dapperConfig.DbConnectionString))
            {
                var clientQuery = await connection.QueryMultipleAsync(ClientSql, new { client = clientId });
                var clients = clientQuery.Read<Model.Client>().AsList();

                if (clients != null && clients.Any())
                {
                    var client = clients.First();

                    var multi = await connection.QueryMultipleAsync(OtherClientInfoSql, new { client = clientId });
                    client.AllowedGrantTypes = multi.Read<Model.ClientGrantType>().AsList();
                    client.RedirectUris = multi.Read<Model.ClientRedirectUri>().AsList();
                    client.AllowedScopes = multi.Read<Model.ClientScope>().AsList();
                    client.ClientSecrets = multi.Read<Model.ClientSecret>().AsList();

                    _logger.LogDebug($"{clientId} found in database: true");
                    return client.ToModel();
                }
            }

            _logger.LogDebug($"{clientId} found in database: false");
            return new Client();
        }
    }
}
