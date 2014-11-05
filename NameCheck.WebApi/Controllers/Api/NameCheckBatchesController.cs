using SuperMassive;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace NameCheck.WebApi
{
    [RoutePrefix("api/namecheckbatches")]
    [Authorize(Roles=Constants.Claims.Roles.AuthenticatedUser)]
    public class NameCheckBatchesController : BaseApiController
    {
        protected NameCheckProvider Provider { get; private set; }

        protected IDataService<NameCheckBatchModel, DescendingSortedGuid> DataService { get; private set; }

        public NameCheckBatchesController(IDataService<NameCheckBatchModel, DescendingSortedGuid> dataService, NameCheckProvider provider)
        {
            Guard.ArgumentNotNull(dataService, "dataService");
            DataService = dataService;
            Guard.ArgumentNotNull(provider, "provider");
            Provider = provider;
        }

        public async Task<IHttpActionResult> Post(NameCheckBatchModel model)
        {
            if (model == null)
            {
                return BadRequest("Missing data");
            }
            if (String.IsNullOrWhiteSpace(model.Value))
            {
                return BadRequest("Missing batch data");
            }

            try
            {
                model = await Provider.NameCheckBatchAsync(model.Value, Constants.DefaultBatchSeparator, EndpointType.Api, Request.GetClientIpAddress());
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