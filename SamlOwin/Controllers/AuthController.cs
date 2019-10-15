using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SamlOwin.Managers;
using Claim = System.IdentityModel.Claims.Claim;
using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

namespace SamlOwin.Controllers
{
    public class AuthController : ApiController
    {
        public AuthController()
        {
        }

        public AuthController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get =>_userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            
            private set =>  _userManager = value; 
        }

        private IAuthenticationManager AuthenticationManager
        {
            get => HttpContext.Current.GetOwinContext().Authentication;
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("LoginCallback")]
        public async Task<HttpResponseMessage> LoginCallback(string returnUrl = "")
        {
            var response = Request.CreateResponse();
            response.Headers.Location = new Uri(HttpRuntime.AppDomainAppPath + "api/auth/success");

            // refreshing url will be null
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                response.Headers.Location = new Uri(HttpRuntime.AppDomainAppPath + "api/auth/failure");
            }

            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            return response;
        }


        [AllowAnonymous]
        [HttpGet]
        [ActionName("Success")]
        public string LoginSuccess()
        {
            var user = AuthenticationManager.User;
            
            
            return ":)";
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("Failure")]
        public string LoginFailure()
        {
            return ":(";
        }
    }
}