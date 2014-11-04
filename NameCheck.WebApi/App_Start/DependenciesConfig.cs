using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.WindowsAzure;
using SuperMassive;
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

            builder.RegisterType<NameCheckCache>().As<IMemoryCache<NameCheckModel>>()
                .InstancePerRequest();

            builder.RegisterType<AuthorizedUserStore>().As<IAuthorizedUserStore>()
                .InstancePerRequest();

            builder.RegisterType<NameCheckProvider>()
                .InstancePerRequest();

            // Register Controllers
            builder.RegisterControllers(Assembly.GetAssembly(typeof(BaseApiController)));
            builder.RegisterApiControllers(Assembly.GetAssembly(typeof(BaseApiController)));

            // Register filters
            builder.RegisterFilterProvider();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            // Register modules
            builder.RegisterModule<MappersModule>();
            builder.RegisterModule(new RepositoriesModule()
            {
                StorageConnectionString = CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.StorageConnectionString)
            });
            builder.RegisterModule<DataServicesModule>();

            // Ioc Container
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }

        class RepositoriesModule : Autofac.Module
        {
            public string StorageConnectionString { get; set; }

            protected override void Load(ContainerBuilder builder)
            {
                builder.Register(x => new NameCheckRepository(StorageConnectionString))
                .As<IRepository<NameCheckEntity>>();

                builder.Register(x => new NameCheckBatchRepository(StorageConnectionString))
                    .As<IRepository<NameCheckBatchEntity>>();
            }
        }

        class MappersModule : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.Register(x => new NameCheckMapper())
                .As<IMapper<NameCheckModel, NameCheckEntity>>();

                builder.Register(x => new NameCheckBatchMapper())
                    .As<IMapper<NameCheckBatchModel, NameCheckBatchEntity>>();
            }
        }

        class DataServicesModule : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<NameCheckDataService>().As<IDataService<NameCheckModel, DescendingSortedGuid>>()
                .InstancePerRequest();

                builder.RegisterType<NameCheckBatchDataService>().As<IDataService<NameCheckBatchModel, DescendingSortedGuid>>()
                    .InstancePerRequest();
            }
        }

    }
}