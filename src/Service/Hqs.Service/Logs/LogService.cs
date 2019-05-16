using System;
using System.Linq;
using System.Threading.Tasks;
using Hqs.Dto;
using Hqs.Dto.Logs;
using Hqs.Helper;
using Hqs.IService;
using Hqs.IService.Logs;
using Hqs.Model.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hqs.Service.Logs
{
    public class LogService : CoreService, ILogService
    {
        #region Ctor

        private readonly DbSet<Log> _logDbSet;
        private readonly HttpContext _httpContext;

        public LogService(DbContext context,
            IHttpContextAccessor httpContextAccessor)
            : base(context)
        {
            _logDbSet = DataContext.Set<Log>();
            _httpContext = httpContextAccessor.HttpContext;
        }

        #endregion

        #region Util

        private void Insert(LogType type, string message)
        {
            var log = Create<Log>();
            log.CreatedBy = "-1";
            log.ModuleName = "(null)";
            log.ActionName = "(null)";
            log.LogType = (int)type;
            log.LogTypeName = type.ToDisplay();
            log.Description = message;
            log.AccessUrl = _httpContext.Request.Host + _httpContext.Request.Path + _httpContext.Request.QueryString;
            log.IpAddress = _httpContext.Connection.RemoteIpAddress.ToString();
            _logDbSet.Add(log);
            SaveChanges();
        }

        private async Task InsertAsync(LogType type, string message)
        {
            var log = Create<Log>();
            log.CreatedBy = "-1";
            log.ModuleName = "(null)";
            log.ActionName = "(null)";
            log.LogType = (int)type;
            log.LogTypeName = type.ToDisplay();
            log.Description = message;
            log.AccessUrl = _httpContext.Request.Path + _httpContext.Request.QueryString;
            log.IpAddress = _httpContext.Connection.RemoteIpAddress.ToString();
            _logDbSet.Add(log);
            await SaveChangesAsync();
        }

        #endregion

        #region Insert

        public void LogDebug(string message)
        {
            Insert(LogType.Debug, message);
        }

        public void LogInfo(string message)
        {
            Insert(LogType.Info, message);
        }

        public void LogWarning(string message)
        {
            Insert(LogType.Warning, message);
        }

        public void LogError(string message)
        {
            Insert(LogType.Error, message);
        }

        public void LogFail(string message)
        {
            Insert(LogType.Fail, message);
        }

        public void LogDebug(string message, Exception ex)
        {
            Insert(LogType.Debug, $"{message}。Exception：{JsonHelper.Serialize(ex)}");
        }

        public void LogInfo(string message, Exception ex)
        {
            Insert(LogType.Info, $"{message}。Exception：{JsonHelper.Serialize(ex)}");
        }

        public void LogWarning(string message, Exception ex)
        {
            Insert(LogType.Warning, $"{message}。Exception：{JsonHelper.Serialize(ex)}");
        }

        public void LogError(string message, Exception ex)
        {
            Insert(LogType.Error, $"{message}。Exception：{JsonHelper.Serialize(ex)}");
        }

        public void LogFail(string message, Exception ex)
        {
            Insert(LogType.Fail, $"{message}。Exception：{JsonHelper.Serialize(ex)}");
        }

        #region Async

        public async Task LogDebugAsync(string message)
        {
           await InsertAsync(LogType.Debug, message);
        }

        public async Task LogInfoAsync(string message)
        {
            await InsertAsync(LogType.Info, message);
        }

        public async Task LogWarningAsync(string message)
        {
            await InsertAsync(LogType.Warning, message);
        }

        public async Task LogErrorAsync(string message)
        {
            await InsertAsync(LogType.Error, message);
        }

        public async Task LogFailAsync(string message)
        {
            await InsertAsync(LogType.Fail, message);
        }

        public async Task LogDebugAsync(string message, Exception ex)
        {
            await InsertAsync(LogType.Debug, $"{message}。Exception：{JsonHelper.Serialize(ex)}");
        }

        public async Task LogInfoAsync(string message, Exception ex)
        {
            await InsertAsync(LogType.Info, $"{message}。Exception：{JsonHelper.Serialize(ex)}");
        }

        public async Task LogWarningAsync(string message, Exception ex)
        {
            await InsertAsync(LogType.Warning, $"{message}。Exception：{JsonHelper.Serialize(ex)}");
        }

        public async Task LogErrorAsync(string message, Exception ex)
        {
            await InsertAsync(LogType.Error, $"{message}。Exception：{JsonHelper.Serialize(ex)}");
        }

        public async Task LogFailAsync(string message, Exception ex)
        {
            await InsertAsync(LogType.Fail, $"{message}。Exception：{JsonHelper.Serialize(ex)}");
        }

        #endregion

        #endregion

        #region Select

        public PageDto<Log> GetList(LogSearch search)
        {
            var query = _logDbSet.OrderByDescending(p => p.CTime).AsQueryable();
            return new PageDto<Log>()
            {
                Count = query.Count(),
                Item = query.Skip(search.PerPage).Take(search.PageSize).ToList()
            };
        }

        #endregion
    }
}
