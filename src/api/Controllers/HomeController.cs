using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    /// <summary>
    /// A simple controller to demonstrate the capabilities of .NET Core Web API
    /// </summary>

    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public class Something
        {
            public string name { get; set; }
            public int value { get; set; }
        }

        [HttpGet]
        [Route("")]
        public ActionResult<Something> Get()
        {
            this._logger.Log(LogLevel.Information, "Getting something");

            return Ok(new Something
            {
                name = "Hello controller",
                value = 10,
            });
        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<Something> GetAll()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Something
            {
                name = $"Something {rng.Next(20)}",
                value = rng.Next(100),
            });
        }
    }
}
