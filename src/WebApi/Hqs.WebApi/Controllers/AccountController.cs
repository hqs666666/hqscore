using System;
using System.Collections.Generic;
using Hqs.Dto.Users;
using Hqs.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Hqs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        #region Ctor

        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        [HttpPost("login")]
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
            var result = HttpHelper.Post(_configuration["AppSetting:AuthServer"] + _configuration["AppSetting:Address"], param);
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

            var result = HttpHelper.Post(_configuration["AppSetting:AuthServer"] + _configuration["AppSetting:Address"], param);
            return Ok(result);
        }
    }
}
