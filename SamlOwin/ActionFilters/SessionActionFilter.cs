using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel.PeerResolvers;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;
using Sustainsys.Saml2.WebSso;

namespace SamlOwin.ActionFilters
{
    public class SessionActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // not on login
            if (actionContext.Request.RequestUri.ToString() != "http://localhost:50229/api/auth/loginCallback")
            {
                if (HttpContext.Current.User.Identity is ClaimsIdentity identity)
                {
                    var sessionIndex = identity.Claims
                        .Where(c => c.Type == "http://Sustainsys.se/Saml2/SessionIndex")
                        .Select(c => c.Value).LastOrDefault();

                    if (sessionIndex == null || MemoryCache.Default.Get(sessionIndex) == null)
                    {
                        HttpContext.Current.GetOwinContext().Authentication.SignOut();
                        
                        HttpContext.Current.User =
                            new GenericPrincipal(new GenericIdentity(string.Empty), null);

                        Console.WriteLine("LOGOUT User {0}", sessionIndex);
                    }
                }
            }

            base.OnActionExecuting(actionContext);
        }

        public static void RegisterSession(ClaimsIdentity claimsIdentity)
        {
            var sessionIndex = claimsIdentity.Claims
                .Where(c => c.Type == "http://Sustainsys.se/Saml2/SessionIndex")
                .Select(c => c.Value).LastOrDefault();
            
            if (sessionIndex != null)
            {
                MemoryCache.Default.Add(sessionIndex, "", new DateTimeOffset(DateTime.Now.AddDays(1)));
            }
            
            Console.WriteLine("LOGIN Session {0}", sessionIndex);
        }
        
        public static void DeregisterSession(ClaimsPrincipal claimsIdentity)
        {
            var sessionIndex = claimsIdentity.Claims
                .Where(c => c.Type == "http://Sustainsys.se/Saml2/SessionIndex")
                .Select(c => c.Value).LastOrDefault();
            
            if (sessionIndex != null)
            {
                MemoryCache.Default.Remove(sessionIndex);
            }
            
            // Sets IsAuthenticated = false for CookieActionFilter
            HttpContext.Current.User =
                new GenericPrincipal(new GenericIdentity(string.Empty), null);

            Console.WriteLine("LOGOUT Session {0}", sessionIndex);
        }
        
        public static Func<HttpRequestData, Saml2Binding> GetSaml2Binding()
        {
            return (request) =>
            {
                Console.WriteLine("GetBinding");

                if (request.Url.ToString() == "http://localhost:50229/api/saml/Acs")
                {
                    return Saml2Binding.Get(Saml2BindingType.HttpPost);
                }

                var sessionIndex = request.User.Claims
                    .Where(c => c.Type == "http://Sustainsys.se/Saml2/SessionIndex")
                    .Select(c => c.Value).LastOrDefault();

                request.User =
                    new GenericPrincipal(new GenericIdentity(string.Empty), null);

                if (sessionIndex != null)
                {
                    MemoryCache.Default.Remove(sessionIndex);
                }

                Console.WriteLine("LOGOUT Session {0}", sessionIndex);

                return null;
            };
        }
    }
}