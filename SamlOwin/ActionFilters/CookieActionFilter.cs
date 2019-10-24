using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;
using Serilog;

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
        public SessionCookeHeaderValue(string name, string value, DateTimeOffset expires) : base(name, value)
        {
            Expires = expires;
            HttpOnly = false;
            Secure = true;
            Path = "/";
        }
    }

    public class CookieActionFilter : AuthorizationFilterAttribute, IAuthorizationFilter
    {
        private const string LogoutAbsolutePath = "/api/auth/logout";

        public static readonly string[] ClaimTypes =
        {
            "expiresAt", "volunteer.id", "volunteer.canApplyCac", "volunteer.canApplyCsc",
            "volunteer.canApplyReac", "volunteer.emailVerified"
        };

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            Log.Logger.Information("CookieActionFilter.ExecuteAuthorizationFilterAsync");

            // actionContext.RequestContext.Principal.Identity.IsAuthenticated is true on logout

            var response = await continuation();

            var identity =
                HttpContext.Current.GetOwinContext()?.Authentication.AuthenticationResponseGrant != null
                    ? HttpContext.Current.GetOwinContext().Authentication.AuthenticationResponseGrant.Identity
                    : actionContext.RequestContext.Principal.Identity as ClaimsIdentity;

            var cookieHeaderValues = new List<CookieHeaderValue>();

            if (identity != null && identity.IsAuthenticated &&
                actionContext.Request.RequestUri.AbsolutePath != LogoutAbsolutePath)
            {
                var expires = DateTimeOffset.Now.AddMinutes(
                    Convert.ToDouble(ConfigurationManager.AppSettings["SessionTimeInMinutes"]));

                // if logout clear cookies
                if (identity.HasClaim(c => c.Type == "expires"))
                {
                    expires = DateTimeOffset.Parse(identity.FindFirstValue("expires"));
                }

                cookieHeaderValues.Add(new SessionCookeHeaderValue("expiresAt", expires.ToString("O"), expires));

                foreach (var claimType in ClaimTypes)
                {
                    // if logout clear cookies
                    if (identity.HasClaim(c => c.Type == claimType))
                    {
                        var claimValue = identity.FindFirstValue(claimType);
                        cookieHeaderValues.Add(new SessionCookeHeaderValue(claimType, claimValue, expires));
                    }
                }
            }
            else
            {
                cookieHeaderValues.AddRange(ClaimTypes.Select(claimType => new ExpiredCookeHeaderValue(claimType))
                    .Cast<CookieHeaderValue>());
            }

            response.Headers.AddCookies(cookieHeaderValues);

            return response;
        }
    }
}