using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace SamlOwin.GuidIdentity
{
    public class ApplicationClaimsIdentityFactory : ClaimsIdentityFactory<ApplicationUser, Guid>
    {
        public override Task<ClaimsIdentity> CreateAsync(UserManager<ApplicationUser, Guid> manager,
            ApplicationUser user, string authenticationType)
        {
            var id = new ClaimsIdentity(authenticationType, UserNameClaimType, RoleClaimType);

            id.AddClaim(new Claim(UserIdClaimType, ConvertIdToString(user.Id),
                ClaimValueTypes.String));

            id.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                "ASP.NET Identity", ClaimValueTypes.String));

            return Task.FromResult(id);
        }
    }
}