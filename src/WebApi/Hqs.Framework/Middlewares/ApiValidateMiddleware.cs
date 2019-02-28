
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hqs.Dto.ResultMsg;
using Hqs.Helper;
using Hqs.IService;
using Microsoft.AspNetCore.Http;

namespace Hqs.Framework.Middlewares
{
    public class ApiValidateMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiValidateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;
            if (string.IsNullOrEmpty(headers[AppConstants.X_CA_KEY]))
            {
                await ForbiddenRequestResultAsync(context, AppErrorCode.AppIdRequired);
                return;
            }

            if (string.IsNullOrEmpty(headers[AppConstants.X_CA_NONCE]))
            {
                await ForbiddenRequestResultAsync(context, AppErrorCode.NonceRequired);
                return;
            }

            if (string.IsNullOrEmpty(headers[AppConstants.X_CA_SIGNATURE]))
            {
                await ForbiddenRequestResultAsync(context, AppErrorCode.SignatureRequired);
                return;
            }

            if (string.IsNullOrEmpty(headers[AppConstants.X_CA_TIMESTAMP]))
            {
                await ForbiddenRequestResultAsync(context, AppErrorCode.TimestampRequired);
                return;
            }

            var requestDate = TimeStampToDate(headers[AppConstants.X_CA_TIMESTAMP].ToString());
            if ((DateTime.Now - requestDate).Minutes >= 10)
            {
                await ForbiddenRequestResultAsync(context, AppErrorCode.InvalidSignature);
                return;
            }
            else
            {
                
            }


            var fromData = new Dictionary<string, object>();
            fromData.Add(AppConstants.X_CA_KEY, headers[AppConstants.X_CA_KEY]);
            fromData.Add(AppConstants.X_CA_NONCE, headers[AppConstants.X_CA_NONCE]);
            fromData.Add(AppConstants.X_CA_TIMESTAMP, headers[AppConstants.X_CA_TIMESTAMP]);

            var queryString = context.Request.QueryString;
            if (queryString.HasValue)
            {
                var queryArray = queryString.Value.TrimStart('?').Split('&');
                foreach (var item in queryArray)
                {
                    var newArray = item.Split('=');
                    fromData.Add(newArray[0], newArray[1]);
                }
            }

            if (context.Request.Method != HttpMethods.Get)
            {
                var fromParam = context.Request.Body.GetString();
                if (!string.IsNullOrEmpty(fromParam))
                {
                    var param = JsonHelper.Deserialize<Dictionary<string, object>>(fromParam);
                    foreach (var item in param)
                    {
                        fromData.Add(item.Key, item.Value);
                    }
                }
            }

            fromData = new Dictionary<string, object>(fromData.OrderBy(p => p.Key));
            var signature = JsonHelper.Serialize(fromData).ToMd5();
            if (!signature.Equals(headers[AppConstants.X_CA_SIGNATURE]))
            {
                await ForbiddenRequestResultAsync(context, AppErrorCode.InvalidSignature);
                return;
            }

            await _next.Invoke(context);
        }

        private ApiResultMsg CreateApiResultMsg(AppErrorCode code,string message = null)
        {
            return new ApiResultMsg()
            {
                StatusCode = (int)code,
                Message = message ?? code.ToDisplay()
            };
        }

        private async Task ForbiddenRequestResultAsync(HttpContext context, AppErrorCode code)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(JsonHelper.Serialize(CreateApiResultMsg(code)));
        }

        private DateTime TimeStampToDate(string timeStamp)
        {
            var stamp = string.Concat(timeStamp,"000");
            return Util.StampToDateTime(stamp);
        }
    }
}
