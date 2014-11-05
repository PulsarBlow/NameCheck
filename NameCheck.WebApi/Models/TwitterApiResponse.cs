

namespace NameCheck.WebApi
{
    public class TwitterApiResponse<T>
    {
        public TwitterApiResponse()
        { }
        public TwitterApiResponse(T content)
            : this(content, null)
        {

        }
        public TwitterApiResponse(T content, TwitterApiError error)
        {
            Content = content;
            Error = error;
        }

        public T Content { get; set; }
        public TwitterApiError Error { get; set; }
    }
}