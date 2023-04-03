using Microsoft.AspNetCore.Mvc;
using blog.Attributes;

namespace blog.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        [ApiKey]
        public IActionResult Get()
        {
            return Ok();
        }

    }
}