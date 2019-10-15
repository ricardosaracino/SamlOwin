using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SamlOwin.Identity;

namespace SamlOwin.Controllers
{
    public class AuthController : ApiController
    {
        private ApplicationSignInManager _signInManager;

        public AuthController()
        {
        }

        public AuthController(ApplicationSignInManager signInManager)
        {
            SignInManager = signInManager;
        }

        private ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            set => _signInManager = value;
        }

        private static IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;

        [AllowAnonymous, HttpGet, ActionName("LoginCallback")]
        public async Task<HttpResponseMessage> LoginCallback(string returnUrl = "")
        {
            var response = Request.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri("https://localhost:44325/api/auth/success");

            // refreshing url will be null
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                response.Headers.Location = new Uri(HttpRuntime.AppDomainAppPath + "https://localhost:44325/api/auth/failure");
                return response;
            }

            // If IsPersistent property of AuthenticationProperties is set to false, then the cookie expiration time is set to Session.
            await SignInManager.ExternalSignInAsync(loginInfo, true);

            return response;
        }

        [Authorize, HttpGet, ActionName("Success")]
        public string LoginSuccess()
        {
            var user = AuthenticationManager.User;
            return ":)";
        }

        [AllowAnonymous, HttpGet, ActionName("Failure")]
        public string LoginFailure()
        {
            return ":(";
        }
    }
}