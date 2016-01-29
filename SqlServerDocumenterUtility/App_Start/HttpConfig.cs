using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace SqlServerDocumenterUtility.App_Start
{
    /// <summary>
    /// Overrides for Http configuration
    /// </summary>
    public class HttpConfig
    {
        /// <summary>
        /// Setup configuration so that the models returned by Web API are camel cased
        /// the same.This so that they match the models Nancy returns allowing the
        /// angular code to handle the response the same way, regardless of the api used
        /// </summary>
        public static void Configure()
        {
            
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
        }
    }
}