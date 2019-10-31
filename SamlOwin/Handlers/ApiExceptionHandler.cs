using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace SamlOwin.Handlers
{
    public class ApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new JsonErrorResult
            {
                Request = context.ExceptionContext.Request,
                Status = "error",
                Message = context.Exception.Message
            };
            base.Handle(context);
        }

        private class JsonErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public string Status { get; set; }

            public string Message { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(
                        $"{{\"status\": \"{Status}\", \"message\": \"{Message}\"}}",
                        Encoding.UTF8,
                        "application/json"
                    ),
                    RequestMessage = Request
                });
            }
        }
    }
}