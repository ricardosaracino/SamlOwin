using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SamlOwin.Handlers;
using Serilog;
using Serilog.Events;

namespace SamlOwin
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            RegisterFormatters(config);
            
            RegisterLogger();
            
            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                "ActionApi",
                "api/{controller}/{action}/{id}",
                new {id = RouteParameter.Optional}
            );
            
            config.Services.Replace(typeof(IHttpControllerSelector), new ApiControllerSelector(config));
            
            config.Services.Replace(typeof(IExceptionHandler), new ApiExceptionHandler());
            
            config.Services.Replace(typeof(IExceptionLogger), new ApiExceptionLogger());
            
            config.MessageHandlers.Add(new CookieHandler());
            
            config.Filters.Add(new GccfAuthorizationFilter());
        }

     

        private static void RegisterFormatters(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        }

        private static void RegisterLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole(LogEventLevel.Debug)
                .CreateLogger();
        }
    }
}