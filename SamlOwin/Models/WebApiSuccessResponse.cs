namespace SamlOwin.Models
{
    /// <inheritdoc/>
    public class ApiSuccessResponse<T> : ApiResponse<T>
    {
        public new string Status = "success";
    }
}