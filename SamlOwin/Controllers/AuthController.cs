using System;
using System.Collections.Generic;
using System.Linq;
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
        private ApplicationUserManager _userManager;

        public AuthController()
        {
        }

        public AuthController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            set => _userManager = value;
        }

        private static IAuthenticationManager AuthenticationManager =>
            HttpContext.Current.GetOwinContext().Authentication;


        [AllowAnonymous]
        [HttpGet]
        [ActionName("LoginCallback")]
        public async Task<HttpResponseMessage> LoginCallback(string returnUrl = "")
        {
            var response = Request.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri("https://dev-ep-pe.csc-scc.gc.ca/site/");

            // refreshing url will be null
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                // i think the url has a error message on it
                response.Headers.Location =
                    new Uri("https://dev-ep-pe.csc-scc.gc.ca/api/auth/error?error=ExternalLoginInfo");
                return response;
            }

            // If IsPersistent property of AuthenticationProperties is set to false, then the cookie expiration time is set to Session.
            var signInStatus = await SignInManager.ExternalSignInAsync(loginInfo, true);

            // https://saml2.sustainsys.com/en/2.0/claims-authentication-manager.html
            AuthenticationManager.User.AddIdentity(loginInfo.ExternalIdentity);

            switch (signInStatus)
            {
                // user null
                case SignInStatus.Failure:
                    response.Headers.Location = new Uri("https://dev-ep-pe.csc-scc.gc.ca/api/auth/error?error=Failure");
                    break;
                case SignInStatus.LockedOut:
                    response.Headers.Location =
                        new Uri("https://dev-ep-pe.csc-scc.gc.ca/api/auth/error?error=LockedOut");
                    break;
                case SignInStatus.RequiresVerification:
                    response.Headers.Location =
                        new Uri("https://dev-ep-pe.csc-scc.gc.ca/api/auth/error?error=RequiresVerification");
                    break;
                case SignInStatus.Success:
                    break;
                default:
                    response.Headers.Location =
                        new Uri("https://dev-ep-pe.csc-scc.gc.ca/api/auth/error?error=RequiresVerification");
                    break;
            }

            return response;
        }

        [Authorize]
        [HttpGet]
        [ActionName("Ping")]
        public Dictionary<string, string> Ping()
        {
            var user = HttpContext.Current.User;

            return AuthenticationManager.User.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("Error")]
        public string Error()
        {
            return ":(";
        }
    }
}