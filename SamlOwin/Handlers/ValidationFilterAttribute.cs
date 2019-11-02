using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SamlOwin.Models;

namespace SamlOwin.Handlers
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    RequestMessage = actionContext.Request,
                    Content = new StringContent(JsonConvert.SerializeObject(
                        new ApiErrorResponse
                        {
                            Message = "The request is invalid.",
                            Data = actionContext.ModelState
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
}