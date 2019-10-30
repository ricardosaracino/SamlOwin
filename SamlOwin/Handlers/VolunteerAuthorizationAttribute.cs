using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using SamlOwin.Identity;

namespace SamlOwin.Handlers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class VolunteerAuthorizationAttribute : AuthorizeAttribute 
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var identity = Thread.CurrentPrincipal.Identity;

            if (identity == null && HttpContext.Current != null)
            {
                identity = HttpContext.Current.User.Identity;
            }

            if (identity != null && identity.IsAuthenticated)
            {

                return identity.GetVolunteerId() !=  Guid.Empty;
            }

            return true;
        }
    }
}