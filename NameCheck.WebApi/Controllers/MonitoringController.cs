using Microsoft.WindowsAzure;
using SuperMassive;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NameCheck.WebApi.Controllers
{
    [SimpleKeyMvcAuthorization(Constants.ConfigurationKeys.AuthorizationSecret)]
    public class MonitoringController : BaseMvcController
    {
        protected IDataService<NameCheckModel, DescendingSortedGuid> DataService { get; private set; }

        public MonitoringController(IDataService<NameCheckModel, DescendingSortedGuid> dataService)
        {
            Guard.ArgumentNotNull(dataService, "dataService");
            DataService = dataService;
        }

        // GET: Monitoring
        public async Task<ActionResult> Index()
        {
            var model = new MonitoringViewModel();

            var rateLimit = await TwitterApiManager.GetRateLimit();
            model.RateLimits = new List<IRateLimit>();
            model.RateLimits.Add(rateLimit.Content);
            try
            {
                model.CheckResults = await DataService.GetCollectionAsync();
            }
            catch (Exception ex)
            {
                model.Error = ex;
            }

            model.Configuration = ReadConfiguration();
            return View(model);
        }

        private MonitoringConfiguration ReadConfiguration()
        {
            MonitoringConfiguration config = new MonitoringConfiguration();
            config.Add("TwitterAccessToken", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterAccessToken));
            config.Add("TwitterAccessTokenSecret", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterAccessTokenSecret));
            config.Add("FacebookAppId", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.FacebookAppId));
            config.Add("FacebookAppSecret", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.FacebookAppSecret));
            return config;
        }
    }
}