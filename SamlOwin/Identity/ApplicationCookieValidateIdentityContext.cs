using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using SamlOwin.Models;

namespace SamlOwin.Identity
{
    public static class ApplicationCookieValidateIdentityContext
    {
        public static Task ApplicationValidateIdentity(CookieValidateIdentityContext context)
        {
            // https://stackoverflow.com/questions/23090706/how-to-know-when-owin-cookie-will-expire

            // validate security stamp for 'sign out everywhere'
            // here I want to verify the security stamp in every 100 seconds.
            // but I choose not to regenerate the identity cookie, so I passed in NULL 
            var stampValidator =

                // todo check portal user still active?
                SecurityStampValidator
                    .OnValidateIdentity<ApplicationUserManager, ApplicationUser, Guid>(
                        TimeSpan.FromMinutes(1),
                        (manager, user) => user.GenerateUserIdentityAsync(manager),
                        user => Guid.Parse(user.GetUserId()));
            
            
            stampValidator.Invoke(context);

            // here we get the cookie expiry time
            var expiresUtc = context.Properties.ExpiresUtc;

            // add the expiry time back to cookie as one of the claims, called 'myExpireUtc'
            // to ensure that the claim has latest value, we must keep only one claim
            // otherwise we will be having multiple claims with same type but different values
            const string claimType = "expiresAt";
            var identity = context.Identity;

            if (identity == null) return Task.FromResult(0);
            if (identity.HasClaim(c => c.Type == claimType))
            {
                var existingClaim = identity.FindFirst(claimType);
                identity.RemoveClaim(existingClaim);
            }

            if (expiresUtc == null) return Task.FromResult(0);
            var newClaim = new Claim(claimType, expiresUtc?.ToString("O"));
            context.Identity.AddClaim(newClaim);

            return Task.FromResult(0);
        }
    }
}