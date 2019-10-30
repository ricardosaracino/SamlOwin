using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Cookies;

namespace SamlOwin.Identity
{
    public static class CustomValidateIdentityContext
    {
        /// <summary>
        /// Not Called on Login
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Task ApplicationValidateIdentity(CookieValidateIdentityContext context)
        {
            // https://stackoverflow.com/questions/23090706/how-to-know-when-owin-cookie-will-expire
            // https://stackoverflow.com/questions/41397898/owin-cookieauthentication-onvalidateidentity-doesnt-call-regenerateidentitycall
            
            var identity = context.Identity;
            
            if (identity.HasClaim(c => c.Type ==  "expires"))
            {
                var existingClaim = identity.FindFirst( "expires");
                identity.RemoveClaim(existingClaim);
            }
            
            if (context.Properties.ExpiresUtc == null) return Task.FromResult(0);
            
            context.Identity.AddClaim(new Claim("expires", context.Properties.ExpiresUtc.ToString()));

            return Task.FromResult(0);
        }
    }
}