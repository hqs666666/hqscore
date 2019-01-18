using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hqs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : BaseController
    {
        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(CreateResultMsg(NickName));
        }

        [AllowAnonymous]
        [HttpGet("uid")]
        public ActionResult GetUser()
        {
            return Ok(CreateResultMsg(UserId));
        }
    }
}
