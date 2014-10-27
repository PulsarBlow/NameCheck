
namespace NameCheck.WebApi
{
    public class ApiError
    {
        public int StatusCode { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
    }
}