using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SamlOwin.Models;

namespace SamlOwin.Handlers
{
    public class WebApiAuthorizationAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var httpStatusCode = actionContext.RequestContext?.Principal.Identity.IsAuthenticated == true
                ? HttpStatusCode.Forbidden
                : HttpStatusCode.Unauthorized;

            actionContext.Response = new HttpResponseMessage(httpStatusCode)
            {
                RequestMessage = actionContext.Request,
                Content = new StringContent(JsonConvert.SerializeObject(
                    new ApiErrorResponse
                    {
                        Message = "Authorization has been denied for this request."
                    },
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }
                ))
            };
        }
    }
}