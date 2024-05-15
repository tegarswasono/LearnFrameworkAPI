using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Services.Configuration;

namespace LearnFrameworkApi.Api.Helpers
{
    public class CurrentUserResolver : ICurrentUserResolver
    {
        private readonly IHttpContextAccessor httpContextAcc;

        public CurrentUserResolver(IHttpContextAccessor httpContextAcc)
        {
            this.httpContextAcc = httpContextAcc;
        }

        public string CurrentId
        {
            get
            {
                return httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? "";
            }
        }
        public string CurrentUsername
        {
            get
            {
                return httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "username")?.Value ?? "";
            }
        }
        public string CurrentEmail
        {
            get
            {
                return httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "email")?.Value ?? "";
            }
        }
        public string CurrentFullname
        {
            get
            {
                return httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "fullname")?.Value ?? "";
            }
        }
        public string CurrentRoles
        {
            get
            {
                return httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "roles")?.Value ?? "";
            }
        }
    }
}
