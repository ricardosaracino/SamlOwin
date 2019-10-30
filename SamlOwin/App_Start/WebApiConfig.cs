﻿using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SamlOwin.Handlers;
using Serilog;
using Serilog.Events;

namespace SamlOwin
{
    public class ApiControllerSelector : DefaultHttpControllerSelector
    {
        public ApiControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
        }

        public override string GetControllerName(HttpRequestMessage request)
        {
            // add logic to remove hyphen from controller name lookup of the controller
            return base.GetControllerName(request).Replace("-", string.Empty);
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            RegisterFormatters(config);

            RegisterFilters(config);

            RegisterLogger();
            
            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                "ActionApi",
                "api/{controller}/{action}/{id}",
                new {id = RouteParameter.Optional}
            );
            
            config.Services.Replace(typeof(IHttpControllerSelector),
                new ApiControllerSelector(config));
        }

        private static void RegisterFilters(HttpConfiguration config)
        {
            config.Filters.Add(new GccfSessionFilter());
            config.Filters.Add(new CookieFilter());
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