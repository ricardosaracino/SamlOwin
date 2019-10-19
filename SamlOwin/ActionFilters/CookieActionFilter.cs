using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;

namespace SamlOwin.ActionFilters
{
    internal class ExpiredCookeHeaderValue : CookieHeaderValue
    {
        public ExpiredCookeHeaderValue(string name) : base(name, "")
        {
            Expires = DateTimeOffset.Now.AddDays(-1);
            HttpOnly = false;
            Secure = true;
            Path = "/";
        }
    }

    internal class SessionCookeHeaderValue : CookieHeaderValue
    {
        public SessionCookeHeaderValue(string name, string value) : base(name, value)
        {
            Expires = DateTimeOffset.Now.AddDays(1);
            HttpOnly = false;
            Secure = true;
            Path = "/";
        }
    }

    public class CookieActionFilter : ActionFilterAttribute
    {
        private readonly string[] _claimTypes =
        {
            "expiresAt", "volunteer.id", "volunteer.canApplyCac", "volunteer.canApplyCsc",
            "volunteer.canApplyReac", "volunteer.emailVerified"
        };

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var identity =
                HttpContext.Current.GetOwinContext()?.Authentication.AuthenticationResponseGrant != null
                    ? HttpContext.Current.GetOwinContext().Authentication.AuthenticationResponseGrant.Identity
                    : HttpContext.Current.User.Identity as ClaimsIdentity;
            
            var cookieHeaderValues = new List<CookieHeaderValue>();

            if (identity == null || !identity.IsAuthenticated)
            {
                cookieHeaderValues.AddRange(_claimTypes.Select(claimType => new ExpiredCookeHeaderValue(claimType))
                    .Cast<CookieHeaderValue>());
            }
            else
            {
                foreach (var claimType in _claimTypes)
                {
                    if (identity.HasClaim(c => c.Type == claimType))
                    {
                        var claimValue = identity.FindFirstValue(claimType);
                        cookieHeaderValues.Add(new SessionCookeHeaderValue(claimType, claimValue));
                    }
                    else
                    {
                        cookieHeaderValues.Add(new ExpiredCookeHeaderValue(claimType));
                    }
                }
            }

            actionExecutedContext.Response.Headers.AddCookies(cookieHeaderValues);

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}