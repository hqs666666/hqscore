using System.Collections.Generic;
using Hqs.Dto.Users;
using Hqs.Framework.Controllers;
using Hqs.Helper;
using Hqs.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hqs.WebApi.Controllers
{
    [Route("api/[controller]")]
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
        public ActionResult Login([FromBody]UserLogin user)
        {
            var param = new Dictionary<string, object>()
            {
                { "client_id", "roclient" },
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
        public ActionResult RefreshToken([FromBody] string token)
        {
            var param = new Dictionary<string, object>()
            {
                { "client_id", "ro.client" },
                { "client_secret","secret"},
                { "grant_type","refresh_token"},
                { "refresh_token",token}
            };

            var result = HttpHelper.Post(_authAdress, param);
            return Ok(result);
        }
    }
}
