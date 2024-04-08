using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Helpers
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser>
    {
        public CustomClaimsPrincipalFactory(UserManager<AppUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("preferred_username", user.UserName ?? ""));
            var role = await UserManager.GetRolesAsync(user);
            identity.AddClaim(new Claim("role", role.First()));
            //identity.AddClaim(new Claim("action_role", user.ActionRoleId.ToString()));

            return identity;
        }
    }
}
