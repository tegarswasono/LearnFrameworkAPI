using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using LearnFrameworkApi.Module.Datas;

namespace LearnFrameworkApi.Api
{
    public class AppAuthorizeAttribute : TypeFilterAttribute
    {
        public AppAuthorizeAttribute(params string[] permissions)
        : base(typeof(AuthorizeActionFilterAttribute))
        {
            Arguments = new object[] { permissions };
        }
    }

    public class AuthorizeActionFilterAttribute : Attribute, IAuthorizationFilter
    {
        public string[] Permissions { get; }
        private readonly AppDbContext _context;

        public AuthorizeActionFilterAttribute(AppDbContext context, params string[] permissions)
        {
            Permissions = permissions;
            _context = context;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new UnauthorizedResult();
            }

            var exist = _context.RoleFunctions
                .Any(x =>
                    x.FunctionId == Permissions[0] &&
                    _context.UserRoles.Where(x => x.UserId == Guid.Parse(userId!)).Select(x => x.RoleId).Contains(x.RoleId)
                );
            if (!exist)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
