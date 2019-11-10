using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SamlOwin.Models;

namespace SamlOwin.Handlers
{
    public class WebApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new JsonErrorResult
            {
                Request = context.ExceptionContext.Request,
                Message = context.Exception.Message
            };
            base.Handle(context);
        }

        private class JsonErrorResult : IHttpActionResult
        {
            public string Message { get; set; }

            public HttpRequestMessage Request { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    RequestMessage = Request,
                    Content = new StringContent(JsonConvert.SerializeObject(
                        new WebApiErrorResponse
                        {
                            Message = "The request is invalid.",
                        },
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }
                    ))
                });
            }
        }
    }
}