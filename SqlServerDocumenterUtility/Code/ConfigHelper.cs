
using System.Configuration;
using System.Web;

namespace SqlServerDocumenterUtility.Code
{
    public class ConfigHelper
    {
        /// <summary>
        /// Method to get the nancy api http host path from the web.config
        /// </summary>
        /// <returns>String: Host path for the nancy api</returns>
        public static string GetNancyPath()
        {
            return ConfigurationManager.AppSettings["NancyApiPath"];
        }       

        /// <summary>
        /// Method to get the app's root url
        /// </summary>
        /// <returns></returns>
        public static string GetAppRootPath()
        {
            return VirtualPathUtility.ToAbsolute("~/");
        }

        /// <summary>
        /// Method to get the value in the config file for if bundled
        /// and minified files should be used.
        /// </summary>
        /// <returns></returns>
        public static string Optimize()
        {
            return ConfigurationManager.AppSettings["OptimizeResources"];
        }
    }
}