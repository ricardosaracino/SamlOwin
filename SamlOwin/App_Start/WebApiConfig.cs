using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SamlOwin.ActionFilters;

namespace SamlOwin
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            RegisterFormatters(config);
            
            RegisterFilters(config);

            // config.EnableCors(new EnableCorsAttribute("http://localhost:4200", "*", "*"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "ActionApi",
                "api/{controller}/{action}/{id}",
                new {id = RouteParameter.Optional}
            );
        }

        private static void RegisterFilters(HttpConfiguration config)
        {
            config.Filters.Add(new SessionActionFilter());
            config.Filters.Add(new CookieActionFilter());
        }   
        
        private static void RegisterFormatters(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        }
    }
}