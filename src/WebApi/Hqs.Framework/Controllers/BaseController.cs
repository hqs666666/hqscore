using System.Linq;
using Hqs.Dto.ResultMsg;
using Hqs.IService;
using Microsoft.AspNetCore.Mvc;

namespace Hqs.Framework.Controllers
{
    [ApiController]
    public class BaseController :ControllerBase
    {
        protected ApiResultMsg CreateResultMsg(object value, string message = null)
        {
            return new ApiResultMsg()
            {
                StatusCode = (int)AppErrorCode.Success,
                Data = value,
                Message = message
            };
        }

        protected virtual bool IsAuthenticated => HttpContext.User.Identity.IsAuthenticated;
        protected virtual string UserId => IsAuthenticated ? HttpContext.User.Claims.First(p => p.Type == "sub").Value : "-1";
        protected virtual string UserName => IsAuthenticated ? HttpContext.User.Claims.First(p => p.Type == "name").Value : string.Empty;
        protected virtual string NickName => IsAuthenticated ? HttpContext.User.Claims.First(p => p.Type == "nick_name")?.Value : string.Empty;
        protected virtual string Email => IsAuthenticated ? HttpContext.User.Claims.First(p => p.Type == "email")?.Value : string.Empty;
    }
}
