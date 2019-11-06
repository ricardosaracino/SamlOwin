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
using Microsoft.AspNet.Identity;
using SamlOwin.Identity;
using Serilog;

namespace SamlOwin.Handlers
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

    public class CookieHandler : DelegatingHandler
    {
        private const string LogoutAbsolutePath = "/api/auth/logout";

        public static readonly string[] CookieNames =
        {
            "session.expiresAt", "session.authenticated", "volunteer.id", "volunteer.canApplyCac",
            "volunteer.canApplyCsc", "volunteer.canApplyReac", "volunteer.emailVerified"
        };

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)

        {
            Log.Logger.Information("CookieHandler.ExecuteAuthorizationFilterAsync");

            var response = await base.SendAsync(request, cancellationToken);

            var identity =
                HttpContext.Current.GetOwinContext()?.Authentication.AuthenticationResponseGrant != null
                    ? HttpContext.Current.GetOwinContext().Authentication.AuthenticationResponseGrant.Identity
                    : request.GetRequestContext()?.Principal.Identity as ClaimsIdentity;

            var cookieHeaderValues = new List<CookieHeaderValue>();

            if (identity != null && identity.IsAuthenticated &&
                // if logout clear cookies the identity is still set until Owin clears it
                request.RequestUri.AbsolutePath != LogoutAbsolutePath)
            {
                // make sure its defaulted because the claim is not set on the login callback
                DateTimeOffset expires;

                if (identity.HasClaim(c => c.Type == CustomClaimTypes.Expires))
                {
                    expires = DateTimeOffset.Parse(identity.FindFirstValue(CustomClaimTypes.Expires));
                }
                else
                {
                    expires = DateTimeOffset.UtcNow.AddMinutes(
                        Convert.ToDouble(ConfigurationManager.AppSettings["SessionTimeInMinutes"]));
                }

                cookieHeaderValues.Add(new SessionCookeHeaderValue("session.expiresAt", expires.ToString("O"),
                    expires));

                cookieHeaderValues.Add(new SessionCookeHeaderValue("session.authenticated", "1", expires));
                cookieHeaderValues.Add(new SessionCookeHeaderValue("volunteer.ready", "1", expires));

                cookieHeaderValues.AddRange(
                    (from claimType in CookieNames
                        where identity.HasClaim(c => c.Type == claimType)
                        select new SessionCookeHeaderValue(claimType, identity.FindFirstValue(claimType), expires)));
            }
            else
            {
                cookieHeaderValues.AddRange(CookieNames.Select(claimType => new ExpiredCookeHeaderValue(claimType)));
            }

            Log.Logger.Information("CookieHandler.ExecuteAuthorizationFilterAsync Headers added");

            response.Headers.AddCookies(cookieHeaderValues);

            return response;
        }
    }
}