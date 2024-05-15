using LearnFrameworkApi.Module;
using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Services.Configuration;
using LearnFrameworkMvc.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace LearnFrameworkApi.Api.Controllers.Common
{
    [Route("api/common/[controller]")]
    [ApiController]
    public class TestRoleController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAcc;
        private readonly ICurrentUserResolver userResolver;
        private readonly AppDbContext context;
        public TestRoleController(ICurrentUserResolver userResolver, AppDbContext context, IHttpContextAccessor httpContextAcc)
        {
            this.userResolver = userResolver;
            this.context = context;
            this.httpContextAcc = httpContextAcc;
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
            //way 1
            var tmp11 = User.Claims;
            var tmp12 = User.Claims.FirstOrDefault(x => x.Type == "username");

            //way 2
            var tmp0 = httpContextAcc?.HttpContext?.User?.Claims;
            string? tmp1 = httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            string? tmp2 = httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string? tmp3 = httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
            string? tmp4 = httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "fullname")?.Value;
            string? tmp5 = httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "roles")?.Value;

            //way 3
            var result = new
            {
                userResolver.CurrentId,
                userResolver.CurrentUsername,
                userResolver.CurrentEmail,
                userResolver.CurrentFullname,
                userResolver.CurrentRoles,
            };
            return Ok(result);
        }

        [HttpGet("Test2")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [AppAuthorize(AvailableModuleFunction.RolesDelete)]
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
