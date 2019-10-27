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
using SamlOwin.ActionFilters;
using SamlOwin.Identity;

namespace SamlOwin.Controllers
{
    public class AuthController : ApiController
    {
        private readonly ApplicationSignInManager _signInManager;

        public AuthController()
        {
            _signInManager = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
        }

        private static IAuthenticationManager AuthenticationManager =>
            HttpContext.Current.GetOwinContext().Authentication;

        [AllowAnonymous]
        [HttpGet]
        [ActionName("LoginCallback")]
        public async Task<HttpResponseMessage> LoginCallback(string returnUrl = "https://dev-ep-pe.csc-scc.gc.ca/site/")
        {
            /**
             * Could create a session in SamlOwin.Identity.ApplicationSignInManager.CreateUserIdentityAsync
             * and have it checked and deleted on soap logout
             */
            var response = Request.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(returnUrl);

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
            var signInStatus = await _signInManager.ExternalSignInAsync(loginInfo, true);

            // required for saml2 single sign out
            AuthenticationManager.User.AddIdentity(loginInfo.ExternalIdentity);

            SessionActionFilter.RegisterSession(loginInfo.ExternalIdentity);

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
        [ActionName("Logout")]
        public HttpResponseMessage Logout(string returnUrl = "https://dev-ep-pe.csc-scc.gc.ca/site/")
        {
            // triggers the saml2 sign out
            AuthenticationManager.SignOut();

            // Dont clear Current.User needed for sign out
            SessionActionFilter.DeregisterSession();

            var response = Request.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(returnUrl);

            return response;
        }

        [Authorize]
        [HttpGet]
        [ActionName("Ping")]
        public Dictionary<string, string> Ping()
        {
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