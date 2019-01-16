
using System.Net;
using Hqs.Dto.ResultMsg;
using Microsoft.AspNetCore.Mvc;

namespace Hqs.AuthServer.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected ApiResultMsg CreateResultMsg(object value,string message = null)
        {
            return new ApiResultMsg()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = value,
                Message = message
            };
        }
    }
}