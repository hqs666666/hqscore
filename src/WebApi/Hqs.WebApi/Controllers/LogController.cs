using Hqs.Dto.Logs;
using Hqs.Framework.Controllers;
using Hqs.IService.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hqs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class LogController : BaseController
    {
        private readonly ILogService _logService;
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("")]
        public ActionResult Get([FromQuery]LogSearch search)
        {
            return Ok(CreateResultMsg(_logService.GetList(search)));
        }
    }
}