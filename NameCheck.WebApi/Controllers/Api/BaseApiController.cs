using System.Web.Http;

namespace NameCheck.WebApi
{
    public abstract class BaseApiController : ApiController
    {

        protected BaseApiController()
        {
        }

        //protected HttpResponseMessage OkResponse<T>(T content)
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK,
        //        JsonConvert.SerializeObject(content, Configuration.Formatters.JsonFormatter.SerializerSettings));
        //}

        //protected HttpResponseMessage BadRequestResponse(string message)
        //{
        //    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiError { Code = (int)HttpStatusCode.BadRequest, Description = message });
        //}

        //protected HttpResponseMessage RateLimitResponse(IRateLimit rateLimit)
        //{
        //    return Request.CreateResponse((HttpStatusCode)429, rateLimit);
        //}

        //protected HttpResponseMessage ServerErrorResponse(string message)
        //{
        //    return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiError { Code = (int)HttpStatusCode.InternalServerError, Description = message });
        //}

        //protected HttpResponseMessage ServerErrorResponse(Exception ex)
        //{
        //    return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiError { Code = (int)HttpStatusCode.InternalServerError, Description = ex.Message, Details = ex.StackTrace });
        //}
    }
}