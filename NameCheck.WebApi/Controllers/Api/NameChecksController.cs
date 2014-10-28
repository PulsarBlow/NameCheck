using SerialLabs;
using SerialLabs.Data;
using System;
using System.Net;
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
        public async Task<IHttpActionResult> Get(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return BadRequest("name is not valid");
            }

            var rateLimit = await TwitterApiManager.GetRateLimit();
            if (rateLimit!=null && rateLimit.Content != null && rateLimit.Content.Remaining == 0)
            {
                return Content((HttpStatusCode)429, rateLimit);
            }


            NameCheckModel model = null;

            try
            {
                model = await NameCheckManager.CheckNameAsync(name, EndpointType.Api);
                model.UserIp = Request.GetClientIpAddress();
                await DataService.SaveAsync(model);
            }
            catch (Exception ex)
            {
                // TODO : Log it
                return InternalServerError();

            }
            return Ok(model);
        }
    }
}