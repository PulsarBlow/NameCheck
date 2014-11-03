using SuperMassive;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace NameCheck.WebApi
{
    [RoutePrefix("api/namechecks")]
    public class NameChecksController : BaseApiController
    {
        //protected IMemoryCache<NameCheckModel> Cache { get; private set; }
        protected IDataService<NameCheckModel, DescendingSortedGuid> DataService { get; private set; }
        protected NameCheckProvider Provider { get; private set; }

        public NameChecksController(IDataService<NameCheckModel, DescendingSortedGuid> dataService, NameCheckProvider provider)
        {
            Guard.ArgumentNotNull(dataService, "dataService");
            DataService = dataService;
            Guard.ArgumentNotNull(provider, "provider");
            Provider = provider;
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IHttpActionResult> Get(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return BadRequest("name is not valid");
            }

            //var cached = Cache.GetItem(name);
            //if (cached != null) { return Ok(cached); }

            //var rateLimit = await TwitterApiManager.GetRateLimit();
            //if (rateLimit != null && rateLimit.Content != null && rateLimit.Content.Remaining == 0)
            //{
            //    return Content((HttpStatusCode)429, rateLimit);
            //}


            NameCheckModel model = null;

            try
            {
                model = await Provider.CheckNameAsync(name, EndpointType.Api);
                model.UserIp = Request.GetClientIpAddress();
                await DataService.SaveAsync(model);
            }
            catch
            {
                // TODO : Log it
                return InternalServerError();

            }
            return Ok(model);
        }
    }
}