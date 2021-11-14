using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Auth.Service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/auth/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Test()
        {
            _logger.LogInformation("Test authentication service successfully!)}");
            return "foo";
        }
    }
}