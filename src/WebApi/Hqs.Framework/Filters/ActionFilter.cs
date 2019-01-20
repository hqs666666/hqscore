using Hqs.IService.Logs;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hqs.Framework.Filters
{
    public class ActionFilter : ActionFilterAttribute
    {
        private readonly ILogService _logService;

        public ActionFilter(ILogService logService)
        {
            _logService = logService;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var message = $"{context.HttpContext.Request.Host + context.HttpContext.Request.Path + context.HttpContext.Request.QueryString}；" +
                          $"请求方式：{context.HttpContext.Request.Method}";

            if (context.Exception == null)
            {
                _logService.LogInfo(message);
            }
            else
            {
                _logService.LogError(message, context.Exception);
            }

            base.OnActionExecuted(context);
        }
    }
}
