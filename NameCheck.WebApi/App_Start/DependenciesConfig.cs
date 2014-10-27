using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.WindowsAzure;
using SerialLabs.Data;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace NameCheck.WebApi
{
    public static class DependenciesConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => new CheckResultRepository(CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.StorageConnectionString)))
                .As<IRepository<CheckResultEntity>>();
            builder.Register(x => new CheckResultMapper())
                .As<IMapper<CheckResultModel, CheckResultEntity>>();
            builder.RegisterType<CheckResultDataService>().As<IDataService<CheckResultModel, DescendingSortedGuid>>();

            // Register Controllers
            builder.RegisterControllers(Assembly.GetAssembly(typeof(BaseApiController)));
            builder.RegisterApiControllers(Assembly.GetAssembly(typeof(BaseApiController)));

            // Register filters
            builder.RegisterFilterProvider();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            // Ioc Container
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }

    }
}