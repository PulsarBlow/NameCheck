using System.Net.Http;
using System.Web;

namespace NameCheck.WebApi
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
                return ((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress;

            return "Unavailable";
        }
    }
}