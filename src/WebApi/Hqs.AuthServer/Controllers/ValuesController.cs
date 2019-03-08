using Hqs.Dto.Users;
using Hqs.Framework.Controllers;
using Hqs.IService.Caches;
using Hqs.IService.Users;
using Microsoft.AspNetCore.Mvc;

namespace Hqs.AuthServer.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;

        public ValuesController(IUserService userService,
            ICacheService cacheService)
            //IEnumerable<ICacheService> cacheService)
        {
            _userService = userService;
            _cacheService = cacheService;
            //_cacheService = cacheService.FirstOrDefault(p => p.GetType() == typeof(RedisService));
        }
        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            var user = _cacheService.Get<UserDto>("user");
            if (user == null)
            {
                user = _userService.GetUser("17602132272", "123456");
                _cacheService.Set("user", user, 24 * 60);
            }
            return Ok(CreateResultMsg(user));
        }
    }
}
