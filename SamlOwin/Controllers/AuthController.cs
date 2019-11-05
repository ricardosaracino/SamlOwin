using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SamlOwin.GuidIdentity;
using SamlOwin.Handlers;
using SamlOwin.Models;

namespace SamlOwin.Controllers
{
    [Authorize]
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly ApplicationSignInManager _signInManager;

        private const string BaseUrl = "https://dev-ep-pe.csc-scc.gc.ca";

        public AuthController()
        {
            _signInManager = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
        }

        private static IAuthenticationManager AuthenticationManager =>
            HttpContext.Current.GetOwinContext().Authentication;

        /// <summary>
        /// Creates Application Cookie, Redirects
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns>RedirectActionResult</returns>
        [AllowAnonymous]
        [HttpGet, Route("saml2/callback")]
        public async Task<RedirectActionResult> SigninCallback(string returnUrl = null)
        {
            /**
             * Could create a session in SamlOwin.Identity.ApplicationSignInManager.CreateUserIdentityAsync
             * and have it checked and deleted on soap logout
             */

            // refreshing url will be null
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return new RedirectActionResult($"{BaseUrl}/en/bad-request?error=ExternalLoginInfo");
            }

            // If IsPersistent property of AuthenticationProperties is set to false, then the cookie expiration time is set to Session.
            var signInStatus = await _signInManager.ExternalSignInAsync(loginInfo, true);

            if (signInStatus != SignInStatus.Success)
            {
                return new RedirectActionResult($"{BaseUrl}/en/bad-request?error={signInStatus:G}");
            }

            // required for saml2 single sign out
            AuthenticationManager.User.AddIdentity(loginInfo.ExternalIdentity);

            GccfAuthorizationFilter.RegisterSession(loginInfo.ExternalIdentity);

            return new RedirectActionResult(returnUrl ?? $"{BaseUrl}/en/#SignIn");
        }

        /// <summary>
        /// Removes Application Cookie, Redirects
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns>RedirectActionResult</returns>
        [HttpGet, Route("logout")]
        public RedirectActionResult Logout(string returnUrl = null)
        {
            // triggers the saml2 sign out
            AuthenticationManager.SignOut();

            // Dont clear Current.User needed for sign out
            GccfAuthorizationFilter.DeregisterSession();

            return new RedirectActionResult(returnUrl ?? $"{BaseUrl}/en/#SignOut");
        }

        /// <summary>
        /// User Claims
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("claims")]
        public Dictionary<string, string> Claims()
        {
            return AuthenticationManager.User.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
        }

        /// <summary>
        /// Ping
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("ping")]
        public WebApiSuccessResponse Ping()
        {
            return new WebApiSuccessResponse();
        }
    }
}