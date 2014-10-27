using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NameCheck.WebApi
{
    public abstract class BaseApiController : ApiController
    {

        protected BaseApiController()
        {
        }

        protected HttpResponseMessage OkResponse<T>(T content)
        {
            return Request.CreateResponse(HttpStatusCode.OK, Json(content, Configuration.Formatters.JsonFormatter.SerializerSettings));
        }

        protected HttpResponseMessage RateLimitResponse(IRateLimit rateLimit)
        {
            return Request.CreateResponse((HttpStatusCode)429, rateLimit);
        }
    }
}