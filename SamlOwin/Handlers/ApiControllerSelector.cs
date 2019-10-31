using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace SamlOwin.Handlers
{
    public class ApiControllerSelector : DefaultHttpControllerSelector
    {
        public ApiControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
        }

        public override string GetControllerName(HttpRequestMessage request)
        {
            // add logic to remove hyphen from controller name lookup of the controller
            return base.GetControllerName(request).Replace("-", string.Empty);
        }
    }
}