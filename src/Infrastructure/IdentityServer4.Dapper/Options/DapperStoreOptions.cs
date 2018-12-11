using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Dapper.Options
{
    /// <summary>
    /// 配置存储
    /// </summary>
    public class DapperStoreOptions
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string DbConnectionString { get; set; }

        /// <summary>
        /// 是否定时清理token
        /// </summary>
        public bool EnableTokenCleanup { get; set; }

        /// <summary>
        /// 清理token周期（s），默认1小时
        /// </summary>
        public int TokenCleanupInterval { get; set; } = 3600;
    }
}
