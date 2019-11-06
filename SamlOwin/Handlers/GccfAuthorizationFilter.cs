using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Serilog;
using Sustainsys.Saml2.Saml2P;
using Sustainsys.Saml2.WebSso;

namespace SamlOwin.Handlers
{
    /// <summary>
    /// This mess is to accomodate the GCCF Global logout.
    /// Long story short there is a session id in MemoryCache that is unset by a SOAP call back.
    /// </summary>
    public class GccfAuthorizationFilter : AuthorizationFilterAttribute, IAuthorizationFilter
    {
        // Saml2Namespaces.Saml2P
        private const string ClaimTypeSessionIndex = "http://Sustainsys.se/Saml2/SessionIndex";

        public async Task<HttpResponseMessage> OOFExecuteAuthorizationFilterAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            Log.Logger.Information("SessionActionFilter.ExecuteAuthorizationFilterAsync");

            // MAKE SURE LOGIN AND LOGOUT HAVE AllowAnonymous
            var skipAuthorization = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();

            var doSignOut = false;

            if (!skipAuthorization && HttpContext.Current.User.Identity is ClaimsIdentity identity)
            {
                var sessionIndex = identity.Claims
                    .Where(c => c.Type == ClaimTypeSessionIndex)
                    .Select(c => c.Value).LastOrDefault();

                if (sessionIndex != null && MemoryCache.Default.Get(sessionIndex) == null)
                {
                    Log.Logger.Information(
                        "SessionActionFilter.OnActionExecuting Unauthorized User {sessionIndex}", sessionIndex);

                    HttpContext.Current.User =
                        new GenericPrincipal(new GenericIdentity(string.Empty), null);

                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

                    // Getting message Authorization has been denied for this request.
                    actionContext.Response.Content = new StringContent("{\"message\":\"Unauthorized\"}");

                    actionContext.Response.Content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/json");

                    doSignOut = true;
                }
            }

            var response = await continuation();

            if (doSignOut)
            {
                Log.Logger.Information(
                    "SessionActionFilter.OnActionExecuting Remove Application Cookie");

                var cookieHeaderValues = new List<CookieHeaderValue>()
                {
                    // manually remove the OWIN ApplicationCookie cookie
                    new ExpiredCookeHeaderValue(ConfigurationManager.AppSettings["ApplicationCookieName"]),
                    new ExpiredCookeHeaderValue(".AspNet.ExternalCookie")
                };

                // remove these too to be safe, i set the cache expiration low and these got out of sync
                cookieHeaderValues.AddRange(CookieHandler.CookieNames
                    .Select(claimType => new ExpiredCookeHeaderValue(claimType)));

                actionContext.Response.Headers.AddCookies(cookieHeaderValues);
            }
            else
            {
                ExtendSession(HttpContext.Current.User.Identity as ClaimsIdentity);
            }

            return response;
        }

        private static void ExtendSession(ClaimsIdentity claimsIdentity)
        {
            Log.Logger.Information("SessionActionFilter.ExtendSession");

            // actionContext.RequestContext.Principal.Identity.IsAuthenticated is true on logout  so we need to call DeregisterSession
            // claimsIdentity is null on AbsolutePath == LoginCallbackAbsolutePath, so we need to call RegisterSession

            var sessionIndex = claimsIdentity?.Claims
                .Where(c => c.Type == ClaimTypeSessionIndex)
                .Select(c => c.Value).LastOrDefault();

            if (sessionIndex == null) return;

            MemoryCache.Default.Add(sessionIndex, $"Active_{sessionIndex}",
                new DateTimeOffset(DateTime.Now.AddMinutes(
                    Convert.ToDouble(ConfigurationManager.AppSettings["SessionTimeInMinutes"]))));

            Log.Logger.Information(
                "SessionActionFilter.OnActionExecuting Extended Session {sessionIndex}", sessionIndex);
        }

        public static void RegisterSession(ClaimsIdentity claimsIdentity)
        {
            Log.Logger.Information("SessionActionFilter.RegisterSession");

            ExtendSession(claimsIdentity);
        }

        public static void DeregisterSession()
        {
            Log.Logger.Information("SessionActionFilter.DeregisterSession");

            var sessionIndex = (HttpContext.Current.User as ClaimsPrincipal)?.Claims
                .Where(c => c.Type == ClaimTypeSessionIndex)
                .Select(c => c.Value).LastOrDefault();

            if (sessionIndex != null)
            {
                MemoryCache.Default.Remove(sessionIndex);
            }

            Log.Logger.Information("SessionActionFilter.DeregisterSession Logout Session {sessionIndex}", sessionIndex);
        }

        public static Func<HttpRequestData, Saml2Binding> GetSaml2Binding()
        {
            return (request) =>
            {
                Log.Logger.Information("SessionActionFilter.GetSaml2Binding {AbsolutePath} {Method}",
                    request.Url.AbsolutePath,
                    request.HttpMethod);

                // ReSharper disable once InvertIf
                if (request.Form.Count > 0
                    && request.Form.Keys.First().ToUpper().Contains("<SOAP-ENV:ENVELOPE")
                    && request.Form.Values.First().ToUpper().Contains("LOGOUTREQUEST"))
                {
                    // I made this Saml2SoapLogoutBinding class 
                    var saml2SoapLogoutBinding = new Saml2SoapLogoutBinding(request);
                    
                    var sessionIndex = saml2SoapLogoutBinding.GetSessionIndex();

                    if (sessionIndex == null) return saml2SoapLogoutBinding;
                    
                    MemoryCache.Default.Remove(sessionIndex);
                        
                    Log.Logger.Information("SessionActionFilter.GetSaml2Binding SOAP Logout {sessionIndex}",
                        sessionIndex);

                    return saml2SoapLogoutBinding;
                }

                //  Post Assertion (/api/saml/Acs)
                return Saml2Binding.Get(request);
            };
        }
    }
}