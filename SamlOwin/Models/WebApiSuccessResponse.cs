namespace SamlOwin.Models
{
    /// <summary>
    /// JSend 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WebApiSuccessResponse<T>
    {
        /// <summary>
        /// Success
        /// </summary>
        public string Status { get; set; } = "Success";

        /// <summary>
        /// JSend Data, can be null
        /// </summary>
        public T Data { get; set; }
    }


    /// <summary>
    /// JSend 
    /// </summary>
    public class WebApiSuccessResponse
    {
        /// <summary>
        /// JSend Status
        /// </summary>
        /// <value>success</value>
        public string Status { get; set; } = "success";

        /// <summary>
        /// JSend Data 
        /// </summary>
        public object Data { get; set; } = new { };
    }
}