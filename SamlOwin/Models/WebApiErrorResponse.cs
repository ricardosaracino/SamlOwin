namespace SamlOwin.Models
{
    /// <summary>
    /// JSend https://github.com/omniti-labs/jsend
    /// </summary>
    public class WebApiErrorResponse
    {
        /// <summary>
        /// JSend Status can be success, fail, error
        /// </summary>
        public readonly string Status = "error";

        /// <summary>
        /// JSend Message
        /// </summary>
        public string Message { get; set; }

        public object Data { get; set; }
    }
}