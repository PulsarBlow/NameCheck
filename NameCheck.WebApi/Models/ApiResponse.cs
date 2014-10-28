

namespace NameCheck.WebApi
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        { }
        public ApiResponse(T content)
            : this(content, null)
        {

        }
        public ApiResponse(T content, ApiError error)
        {
            Content = content;
            Error = error;
        }

        public T Content { get; set; }
        public ApiError Error { get; set; }
    }
}