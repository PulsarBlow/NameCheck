using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NameCheck.WebApi
{
    public class AvailabilitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> Get(string id)
        {
            var rateLimit = await TwitterApiManager.GetRateLimit();
            if (rateLimit.Remaining == 0)
            {
                return RateLimitResponse(rateLimit);
            }

            return OkResponse(new AvailabilityResult { DomainCom = true, Twitter = true, Facebook = true });
        }
    }
}