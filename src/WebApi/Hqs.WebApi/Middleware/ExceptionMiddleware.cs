using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Hqs.WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<Startup> _logService;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(ILogger<Startup> logService,
            RequestDelegate next)
        {
            _logService = logService;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var message = $"{context.Request.Host + context.Request.Path}；请求方式：{context.Request.Method}";
                _logService.LogInformation(message);
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                var reader = new StreamReader(context.Request.Body);
                var requestBody = await reader.ReadToEndAsync();
                var message = $"{context.Request.Host + context.Request.Path + context.Request.QueryString}；" +
                              $"请求方式：{context.Request.Method}；请求参数：{requestBody}；";

                _logService.LogError(message, ex);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
