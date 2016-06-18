using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace crisicheckinweb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = 
                new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Converters = new List<JsonConverter>()
            {
                { new StringEnumConverter() }
            };
        }
    }
}
