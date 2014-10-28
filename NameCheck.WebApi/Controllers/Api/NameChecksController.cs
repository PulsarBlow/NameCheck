using SerialLabs;
using SerialLabs.Data;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NameCheck.WebApi
{
    [RoutePrefix("api/namechecks")]
    public class NameChecksController : BaseApiController
    {
        protected IDataService<NameCheckModel, DescendingSortedGuid> DataService;

        public NameChecksController(IDataService<NameCheckModel, DescendingSortedGuid> dataService)
        {
            Guard.ArgumentNotNull(dataService, "dataService");
            DataService = dataService;
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<HttpResponseMessage> Get(string name)
        {
            if(String.IsNullOrWhiteSpace(name))
            {
                return BadRequestResponse("name is not valid");
            }

            var rateLimit = await TwitterApiManager.GetRateLimit();
            if (rateLimit.Remaining == 0)
            {
                return RateLimitResponse(rateLimit);
            }


            NameCheckModel model = null;

            try
            {
                model = await NameCheckManager.CheckNameAsync(name, EndpointType.Api);
                await DataService.SaveAsync(model);
            }
            catch(Exception ex)
            {
                // TODO : Log it
                return ServerErrorResponse("An error occured :/");

            }
            return OkResponse(model);
        }
    }
}