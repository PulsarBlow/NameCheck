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

            builder.Register(x => new NameCheckRepository(CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.StorageConnectionString)))
                .As<IRepository<NameCheckEntity>>();

            builder.Register(x => new NameCheckMapper())
                .As<IMapper<NameCheckModel, NameCheckEntity>>();

            builder.RegisterType<NameCheckDataService>().As<IDataService<NameCheckModel, DescendingSortedGuid>>()
                .InstancePerRequest();

            // Register Controllers
            builder.RegisterControllers(Assembly.GetAssembly(typeof(BaseApiController)));
            builder.RegisterApiControllers(Assembly.GetAssembly(typeof(BaseApiController)));

            // Register filters
            builder.RegisterFilterProvider();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            // Register web types
            //builder.RegisterModule<AutofacWebTypesModule>();

            // Ioc Container
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }

    }
}