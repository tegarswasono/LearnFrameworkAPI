using LearnFrameworkApi.Module.Services.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace LearnFrameworkApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICurrentUserResolver userResolver;
        public TestController(ICurrentUserResolver userResolver)
        {
            this.userResolver = userResolver;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Ok("Index");
        }
        [HttpGet("Test1")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        public ActionResult Test1() 
        {
            return Ok(new
            {
                userResolver.CurrentUsername,
                userResolver.CurrentAppRole,
            });
        }
        [HttpGet("Test2")]
        public ActionResult Test2()
        {
            return Ok("Test2");
        }
        [HttpGet("Test3")]
        public ActionResult Test3()
        {
            return Ok("Test3");
        }
        [HttpGet("Test4")]
        public ActionResult Test4()
        {
            return Ok("Test4");
        }
    }
}
