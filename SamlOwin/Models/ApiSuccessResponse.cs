namespace SamlOwin.Models
{
    public class ApiSuccessResponse<T> : ApiResponse<T>
    {
        public new string Status = "success";
    }
}