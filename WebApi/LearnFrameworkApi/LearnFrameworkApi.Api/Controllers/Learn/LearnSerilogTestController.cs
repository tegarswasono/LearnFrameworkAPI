using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LearnFrameworkApi.Api.Controllers.Learn
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnSerilogTestController : ControllerBase
    {
        private readonly ILogger<LearnSerilogTestController> _logger;
        public LearnSerilogTestController(ILogger<LearnSerilogTestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("info 2");
            _logger.LogWarning("Warning 2");
            _logger.LogError("Error 2");
            return Ok("OKE");
        }
    }
}
