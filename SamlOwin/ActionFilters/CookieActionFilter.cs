using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Filters;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;

namespace SamlOwin.ActionFilters
{
    internal class ExpiredCookeHeaderValue : CookieHeaderValue
    {
        public ExpiredCookeHeaderValue(string name, string value) : base(name, value)
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
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var cookieHeaderValues = new List<CookieHeaderValue>();

            var expiration = GetExpiresAt();

            if (expiration.IsEmpty())
            {
                cookieHeaderValues.Add(new ExpiredCookeHeaderValue("expiresAt", ""));
            }
            else
            {
                cookieHeaderValues.Add(new SessionCookeHeaderValue("expiresAt", expiration));
            }

            actionExecutedContext.Response.Headers.AddCookies(cookieHeaderValues);

            base.OnActionExecuted(actionExecutedContext);
        }

        private static string GetExpiresAt()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            const string claimType = "ExpiresAtIso"; // see ApplicationCookieValidateIdentityContext

            if (identity != null && identity.HasClaim(c => c.Type == claimType))
            {
                return identity.FindFirstValue(claimType);
            }

            return "";
        }
    }
}