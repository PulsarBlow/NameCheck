using System.Web.Mvc;
using System.Web.Routing;

namespace NameCheck.WebApi
{
    public static class MvcConfig
    {
        public static void Register(RouteCollection routes)
        {
            ConfigureRoutes(routes);
        }

        private static void ConfigureRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}