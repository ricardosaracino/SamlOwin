namespace SamlOwin.Models
{
    /// <summary>
    /// JSend https://github.com/omniti-labs/jsend
    /// </summary>
    public class ApiResponse <T>
    {
        /// <summary>
        /// JSend Status can be success, fail, error
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// JSend Data, can be null
        /// </summary>
        public T Data { get; set; }
    }
}
