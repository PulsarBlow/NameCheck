using Microsoft.WindowsAzure;
using SerialLabs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NameCheck.WebApi.Controllers
{
    [MonitoringAuthorization(Constants.ConfigurationKeys.MonitoringSecret)]
    public class MonitoringController : Controller
    {
        protected IRepository<CheckResultEntity> Repository { get; private set; }
        protected IMapper<CheckResultModel, CheckResultEntity> Mapper { get; private set; }

        public MonitoringController(IRepository<CheckResultEntity> repository, IMapper<CheckResultModel, CheckResultEntity> mapper)
        {
            Guard.ArgumentNotNull(repository, "repository");
            Repository = repository;
            Guard.ArgumentNotNull(mapper, "mapper");
            Mapper = mapper;
        }

        // GET: Monitoring
        public async Task<ActionResult> Index()
        {
            var model = new MonitoringModel();

            model.RateLimits = new List<IRateLimit>();
            model.RateLimits.Add(await TwitterApiManager.GetRateLimit());
            IList<CheckResultEntity> entities = null;
            try
            {
                entities = await Repository.GetCollectionAsync();
            }
            catch (Exception ex)
            {
                model.LastKnownException = ex;
            }

            if (entities != null)
            {
                model.CheckResults = Mapper.ToModel(entities);
            }
            model.Configuration = ReadConfiguration();
            return View(model);
        }

        private MonitoringConfiguration ReadConfiguration()
        {
            MonitoringConfiguration config = new MonitoringConfiguration();
            config.Add("TwitterConsumerKey", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterConsumerKey));
            config.Add("TwitterConsumerSecret", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterConsumerSecret));
            config.Add("TwitterAccessToken", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterAccessToken));
            config.Add("TwitterAccessTokenSecret", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterAccessTokenSecret));
            config.Add("FacebookAppId", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.FacebookAppId));
            config.Add("FacebookAppSecret", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.FacebookAppSecret));
            config.Add("StorageConnectionString", CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.StorageConnectionString));
            return config;
        }
    }
}