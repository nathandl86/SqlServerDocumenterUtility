
using SqlServerDocumenterUtility.App_Start;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SqlServerDocumenterUtility
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            AutoFacConfig.ConfigureWebApi();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            HttpConfig.Configure();
            
        }
    }
}
