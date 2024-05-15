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
            identity.AddClaim(new Claim("id", user.Id.ToString() ?? ""));
            identity.AddClaim(new Claim("username", user.UserName ?? ""));
            identity.AddClaim(new Claim("email", user.Email?? ""));
            identity.AddClaim(new Claim("fullname", user.FullName ?? ""));

            var role = await UserManager.GetRolesAsync(user);
            string roles = string.Join(",", role.ToList());
            identity.AddClaim(new Claim("roles", roles));
            return identity;
        }
    }
}
