using System;
using System.IO;
using System.Threading.Tasks;
using Hqs.Helper;
using Hqs.IService.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hqs.Framework.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var message = $"{context.Request.Host + context.Request.Path + context.Request.QueryString}；" +
                          $"请求方式：{context.Request.Method}";
            try
            {
                await Logger.LogInfoAsync(message);
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await Logger.LogErrorAsync(message,ex);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }

        protected ILogService Logger => LogService();

        private ILogService LogService()
        {
            var provider = DIHelper.ServiceProvider.CreateScope();
            return provider.ServiceProvider.GetService<ILogService>();
        }
    }
}
