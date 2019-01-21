using Hqs.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Hqs.AuthServer.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        [HttpPost]
        public ActionResult Add([FromBody] string id)
        {
            return Ok();
        }
    }
}