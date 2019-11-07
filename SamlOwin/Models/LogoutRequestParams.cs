namespace SamlOwin.Models
{
    public class LogoutRequestParams
    {
        /// <summary>
        /// Return Url on Successful Login
        /// </summary>
        public string ReturnUrl { get; set; }
        
        /// <summary>
        /// Return Url on Error
        /// </summary>
        public string ErrorUrl { get; set; }
        
        /// <summary>
        /// Return Url on User Store Failure
        /// </summary>
        public string UnauthorizedUrl { get; set; }
    }
}