using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SamlOwin.Identity;
using SamlOwin.Models;

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