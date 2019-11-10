using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SamlOwin.GuidIdentity;
using SamlOwin.Handlers;
using SamlOwin.Models;
using Serilog;

namespace SamlOwin.Controllers
{
    [Authorize]
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly ApplicationSignInManager _signInManager;

        private static readonly string BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];

        public AuthController()
        {
            _signInManager = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
        }

        private static IAuthenticationManager AuthenticationManager =>
            HttpContext.Current.GetOwinContext().Authentication;

        /// <summary>
        /// Creates Application Cookie, Redirects
        /// </summary>
        /// <returns>RedirectActionResult</returns>
        [AllowAnonymous]
        [HttpGet, Route("saml2/callback")]
        public async Task<RedirectActionResult> SigninCallback([FromUri] SigninCallbackRequestParams requestParams)
        {
            /**
             * Could create a session in SamlOwin.Identity.ApplicationSignInManager.CreateUserIdentityAsync
             * and have it checked and deleted on soap logout
             */

            var returnUrl = requestParams?.ReturnUrl ?? $"{BaseUrl}/en/";
            var errorUrl = requestParams?.ErrorUrl ?? $"{BaseUrl}/en/bad-request";
            var unauthorizedUrl = requestParams?.UnauthorizedUrl ?? $"{BaseUrl}/en/unauthorized";
            var samlError = requestParams?.Error;

            if (samlError != null)
            {
                return new RedirectActionResult($"{errorUrl}?error=SamlSignInError");
            }

            try
            {
                // refreshing url will be null
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

                if (loginInfo == null)
                {
                    return new RedirectActionResult($"{errorUrl}?error=ExternalLoginInfo");
                }

                // If IsPersistent property of AuthenticationProperties is set to false, then the cookie expiration time is set to Session.
                var signInStatus = await _signInManager.ExternalSignInAsync(loginInfo, true);

                if (signInStatus != SignInStatus.Success)
                {
                    return new RedirectActionResult($"{unauthorizedUrl}?error={signInStatus:G}");
                }

                // required for saml2 single sign out
                AuthenticationManager.User.AddIdentity(loginInfo.ExternalIdentity);

                GccfAuthorizationFilter.RegisterSession(loginInfo.ExternalIdentity);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "");

                return new RedirectActionResult($"{errorUrl}?error=Exception");
            }

            return new RedirectActionResult($"{returnUrl}#SignIn");
        }

        /// <summary>
        /// Removes Application Cookie, Redirects
        /// </summary>
        /// <returns>RedirectActionResult</returns>
        [AllowAnonymous]
        [HttpGet, Route("logout")]
        public RedirectActionResult Logout([FromUri] LogoutRequestParams requestParams)
        {
            var returnUrl = requestParams?.ReturnUrl ?? $"{BaseUrl}/en/";
            var errorUrl = requestParams?.ErrorUrl ?? $"{BaseUrl}/en/bad-request";
            var unauthorizedUrl = requestParams?.UnauthorizedUrl ?? $"{BaseUrl}/en/unauthorized";

            // AllowAnonymous so we can redirect to unauthorized instead of returning json
            if (!User.Identity.IsAuthenticated)
            {
                return new RedirectActionResult($"{unauthorizedUrl}?error=IsAuthenticated");
            }

            try
            {
                // triggers the saml2 sign out
                AuthenticationManager.SignOut();

                // Dont clear Current.User needed for sign out
                GccfAuthorizationFilter.DeregisterSession();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "");

                return new RedirectActionResult($"{errorUrl}?error=Exception");
            }

            // todo CookieHandler has this route hardcoded to clear cookies
            return new RedirectActionResult(returnUrl);
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