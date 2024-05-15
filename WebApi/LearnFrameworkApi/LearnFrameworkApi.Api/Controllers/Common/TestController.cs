using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Services.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace LearnFrameworkApi.Api.Controllers.Common
{
    [Route("api/common/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICurrentUserResolver userResolver;
        private readonly AppDbContext context;
        public TestController(ICurrentUserResolver userResolver, AppDbContext context)
        {
            this.userResolver = userResolver;
            this.context = context;
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
                userResolver.CurrentRoles,
            });
        }
        [HttpGet("Test2")]
        public ActionResult Test2()
        {

            for (int a = 1; a < 40; a++)
            {
                string user = "User" + a;
                var tmp = new AppUser()
                {
                    UserName = user,
                    Email = user + "@gmail.com",
                    FullName = user,
                    PasswordHash = user
                };
                context.Users.Add(tmp);
            }
            context.SaveChanges();
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
