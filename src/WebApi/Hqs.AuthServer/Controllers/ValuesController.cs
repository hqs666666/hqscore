﻿using Hqs.IService.Users;
using Microsoft.AspNetCore.Mvc;

namespace Hqs.AuthServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUserService _userService;

        public ValuesController(IUserService userService)
        {
            _userService = userService;
        }
        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            var name = _userService.GetUser("EEDD685AF17E49348991C130ECEED1B8");
            return new JsonResult(name);
        }
    }
}
