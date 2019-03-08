using System.Collections.Generic;
using Hqs.Dto.Users;
using Hqs.Framework.Controllers;
using Hqs.Helper;
using Hqs.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hqs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : BaseController
    {
        #region Ctor

        private readonly string _authAdress;

        public AccountController(IOptions<AppSetting> options)
        {
            var setting = options.Value;
            _authAdress = setting.AuthConfig.AuthServer + setting.AuthConfig.AuthAddress;
        }

        #endregion

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody]UserLogin user)
        {
            var param = new Dictionary<string, object>()
            {
                { "client_id", "ro.client" },
                { "client_secret","secret"},
                { "grant_type","password"},
                { "username",user.Username},
                { "password",user.Password},
                { "scope","api1 offline_access"}
            };
            var result = HttpHelper.Post(_authAdress, param);
            return Ok(result);
        }

        [HttpPost("refreshtoken")]
        [AllowAnonymous]
        public ActionResult RefreshToken([FromBody]UserLogin user)
        {
            var param = new Dictionary<string, object>()
            {
                { "client_id", "ro.client" },
                { "client_secret","secret"},
                { "grant_type","refresh_token"},
                { "refresh_token",user.Token}
            };

            var result = HttpHelper.Post(_authAdress, param);
            return Ok(result);
        }

        [HttpGet("users")]
        public ActionResult GetUsers()
        {
            return Ok(CreateResultMsg(UserId));
        }
    }
}
