using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Hqs.AuthServer.Middleware
{
    public class ExceptionMiddleware
    {
        public readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger,
            RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                var lReader = new StreamReader(context.Request.Body);
                var lRequestBody = await lReader.ReadToEndAsync();
                var lMessage = $"{context.Request.Host + context.Request.Path}；请求方式：{context.Request.Method}；请求参数：{lRequestBody}；";

                _logger.LogError(lMessage);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
