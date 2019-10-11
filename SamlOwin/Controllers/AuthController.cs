using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SamlOwin.Managers;
using Claim = System.IdentityModel.Claims.Claim;
using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

namespace SamlOwin.Controllers
{
    public class AuthController : ApiController
    {
        /*public AuthController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
        
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }*/

        private IAuthenticationManager AuthenticationManager
        {
            get => HttpContext.Current.GetOwinContext().Authentication;
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("LoginCallback")]
        public async Task<ExternalLoginInfo> ExternalLoginCallback(string returnUrl = "")
        {
           // var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            //var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            var loginInfo = await AuthenticationManager.AuthenticateAsync(AuthenticationTypes.X509);
            
            
            
            var ctx = HttpContext.Current.GetOwinContext();
            var result = ctx.Authentication.AuthenticateAsync("ExternalCookie").Result;
            ctx.Authentication.SignOut("ExternalCookie");
 
            var claims = result.Identity.Claims.ToList();
            //claims.Add(new Claim(ClaimTypes.AuthenticationMethod, provider));
 
            var ci = new ClaimsIdentity(claims, "Cookie");
            ctx.Authentication.SignIn(ci);
 
     
            
            
            return null;
        }
    }
}