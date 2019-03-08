
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hqs.Dto.ResultMsg;
using Hqs.Helper;
using Hqs.IService;
using Hqs.IService.Caches;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

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
            if (context.Request.Method == HttpMethods.Options)
            {
                await _next.Invoke(context);
                return;
            }

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

            //验证随机串和时间戳
            var stamp = headers[AppConstants.X_CA_TIMESTAMP].ToString();
            var reqNonce = headers[AppConstants.X_CA_NONCE].ToString();
            var requestDate = TimeStampToDate(stamp);
            if ((DateTime.Now - requestDate).Minutes >= 10)
            {
                await ForbiddenRequestResultAsync(context, AppErrorCode.InvalidTimestamp);
                return;
            }
            else
            {
                var nonce = _cacheService.Get<string>($"nonce-{stamp}");
                if (!string.IsNullOrEmpty(nonce) && nonce.Equals(reqNonce))
                {
                    await ForbiddenRequestResultAsync(context, AppErrorCode.InvalidNonce);
                    return;
                }

                _cacheService.Set($"nonce-{stamp}", reqNonce, 10 * 60);
            }

            //验证签名
            //if (!CalcSign(context).Equals(headers[AppConstants.X_CA_SIGNATURE].ToString()))
            //{
            //    await ForbiddenRequestResultAsync(context, AppErrorCode.SignatureRequired);
            //    return;
            //}

            await _next.Invoke(context);
        }

        #region Util

        private string CalcSign(HttpContext context)
        {
            var fromData = new Dictionary<string, object>();
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
            var signature = JsonHelper.Serialize(fromData).ToMd5().ToUpper();
            return signature;
        }

        private ApiResultMsg CreateApiResultMsg(AppErrorCode code, string message = null)
        {
            return new ApiResultMsg()
            {
                StatusCode = (int)code,
                Message = message ?? code.ToDisplay()
            };
        }

        private async Task ForbiddenRequestResultAsync(HttpContext context, AppErrorCode code)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(JsonHelper.Serialize(CreateApiResultMsg(code)));
        }

        private DateTime TimeStampToDate(string timeStamp)
        {
            return Util.StampToDateTime(timeStamp);
        }

        #endregion


        private ICacheService _cacheService => CacheService();

        private ICacheService CacheService()
        {
            var provider = DIHelper.ServiceProvider.CreateScope();
            return provider.ServiceProvider.GetService<ICacheService>();
        }
    }
}
