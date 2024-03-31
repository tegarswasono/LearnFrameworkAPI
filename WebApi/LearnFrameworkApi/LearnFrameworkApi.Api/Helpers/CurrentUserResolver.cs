﻿using LearnFrameworkApi.Module.Datas.Entities.Configuration;
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

        public string CurrentUsername
        {
            get
            {
                return httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value ?? "";
            }
        }

        public string CurrentAppRole
        {
            get
            {
                return httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "role")?.Value ?? "";
            }
        }

        public string CurrentActionRole
        {
            get
            {
                return httpContextAcc?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "action_role")?.Value ?? "";
            }
        }

        public AppUser? CurrentAppUser => throw new NotImplementedException();
    }
}