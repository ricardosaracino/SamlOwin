using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace SamlOwin.Handlers
{
    public class RedirectActionResult : IHttpActionResult
    {
        private readonly string _returnUrl;

        public RedirectActionResult(string returnUrl)
        {
            _returnUrl = returnUrl;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage {StatusCode = HttpStatusCode.Redirect};
            
            response.Headers.Location = new Uri(_returnUrl);

            return Task.FromResult(response);
        }
    }
}