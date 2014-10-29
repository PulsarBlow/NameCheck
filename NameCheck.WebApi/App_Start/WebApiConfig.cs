using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Http.Cors;

namespace NameCheck.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ConfigureCors(config);
            ConfigureRoutes(config);
            ConfigureFormatters(config);
        }

        private static void ConfigureCors(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "GET");
            config.EnableCors(cors);
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

    }
}
