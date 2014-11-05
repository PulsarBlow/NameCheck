using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace NameCheck.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ConfigureCors(config);
            ConfigureRoutes(config);
            ConfigureFormatters(config);
            ConfigurateFilters(config);
            //disable host-level authentication inside the Web API pipeline
            config.SuppressHostPrincipal();
        }

        private static void ConfigureCors(HttpConfiguration config)
        {
            config.SetCorsPolicyProviderFactory(new CorsPolicyProviderFactory(
                CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.CorsAllowOrigins),
                CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.CorsAllowMethods),
                CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.CorsAllowHeaders)));
            config.EnableCors();
        }

        private static void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void ConfigureFormatters(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.None;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private static void ConfigurateFilters(HttpConfiguration config)
        {
            config.Filters.Add(new TokenBasedAuthenticationAttribute());
        }


    }
}
