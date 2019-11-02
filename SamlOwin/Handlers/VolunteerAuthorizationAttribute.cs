using System;
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
            if (actionContext.RequestContext?.Principal.Identity.IsAuthenticated == true)
            {
                return actionContext.RequestContext.Principal.Identity.GetVolunteerId() != Guid.Empty;
            }

            return false;
        }
    }
}