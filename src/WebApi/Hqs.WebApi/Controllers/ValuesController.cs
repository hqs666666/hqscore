using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hqs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Get()
        {
            return new JsonResult(new{});
        }
    }
}
