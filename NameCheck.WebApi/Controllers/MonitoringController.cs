using Microsoft.WindowsAzure;
using SuperMassive;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NameCheck.WebApi
{
    [Authorize]
    public class MonitoringController : BaseMvcController
    {
        protected IDataService<NameCheckModel, DescendingSortedGuid> NameCheckDataService { get; private set; }
        protected IDataService<NameCheckBatchModel, DescendingSortedGuid> NameCheckBatchDataService { get; private set; }

        public MonitoringController(IDataService<NameCheckModel, DescendingSortedGuid> nameCheckDataService, IDataService<NameCheckBatchModel, DescendingSortedGuid> nameCheckBatchDataService)
        {
            Guard.ArgumentNotNull(nameCheckDataService, "nameCheckDataService");
            NameCheckDataService = nameCheckDataService;
            Guard.ArgumentNotNull(nameCheckBatchDataService, "nameCheckBatchDataService");
            NameCheckBatchDataService = nameCheckBatchDataService;
        }

        // GET: Monitoring
        public async Task<ActionResult> Index()
        {
            var model = new MonitoringViewModel();
            TwitterApiResponse<Dictionary<string, RateLimit>> response = await TwitterApiManager.GetRateLimits();
            model.RateLimits = response.Content;
            try
            {
                model.LastNameChecks = await NameCheckDataService.GetCollectionAsync();
                model.LastNameCheckBatches = await NameCheckBatchDataService.GetCollectionAsync();
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
            config.Add("GoogleAppId", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.GoogleAppId));
            config.Add("TwitterAccessToken", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterAccessToken));
            config.Add("CorsAllowOrigins", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.CorsAllowOrigins));
            config.Add("CorsAllowMethods", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.CorsAllowMethods));
            config.Add("CorsAllowHeaders", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.CorsAllowHeaders));
            config.Add("CacheDurationMin", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.CacheDurationMin));
            return config;
        }
    }
}