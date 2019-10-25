using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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
        public SessionCookeHeaderValue(string name, string value, DateTimeOffset? expires = null) : base(name, value)
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
            "session.expiresAt", "session.authenticated", "volunteer.id", "volunteer.canApplyCac",
            "volunteer.canApplyCsc", "volunteer.canApplyReac", "volunteer.emailVerified"
        };

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            Log.Logger.Information("CookieActionFilter.ExecuteAuthorizationFilterAsync");

            var response = await continuation();

            var identity =
                HttpContext.Current.GetOwinContext()?.Authentication.AuthenticationResponseGrant != null
                    ? HttpContext.Current.GetOwinContext().Authentication.AuthenticationResponseGrant.Identity
                    : actionContext.RequestContext.Principal.Identity as ClaimsIdentity;

            var cookieHeaderValues = new List<CookieHeaderValue>();

            if (identity != null && identity.IsAuthenticated &&
                // if logout clear cookies the identity is still set until Owin clears it
                actionContext.Request.RequestUri.AbsolutePath != LogoutAbsolutePath)
            {
                // make sure its defaulted because the claim is not set on the login callback
                var expires = DateTimeOffset.Now.AddMinutes(
                    Convert.ToDouble(ConfigurationManager.AppSettings["SessionTimeInMinutes"]));


                if (identity.HasClaim(c => c.Type == "expires"))
                {
                    expires = DateTimeOffset.Parse(identity.FindFirstValue("expires"));
                }

                cookieHeaderValues.Add(new SessionCookeHeaderValue("session.expiresAt", expires.ToString("O"),
                    expires));

                cookieHeaderValues.Add(new SessionCookeHeaderValue("session.authenticated", "1", expires));

                cookieHeaderValues.AddRange(
                    (from claimType in ClaimTypes
                        where identity.HasClaim(c => c.Type == claimType)
                        select new SessionCookeHeaderValue(claimType, identity.FindFirstValue(claimType), expires)));
            }
            else
            {
                cookieHeaderValues.AddRange(ClaimTypes.Select(claimType => new ExpiredCookeHeaderValue(claimType)));
            }

            response.Headers.AddCookies(cookieHeaderValues);

            return response;
        }
    }
}