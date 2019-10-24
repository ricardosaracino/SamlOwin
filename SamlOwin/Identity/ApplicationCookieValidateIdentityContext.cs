using System;
using System.Configuration;
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