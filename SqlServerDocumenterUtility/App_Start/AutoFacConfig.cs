using Autofac;
using Autofac.Integration.WebApi;
using SqlServerDocumenterUtility.Data;
using System.Reflection;
using System.Web.Http;

namespace SqlServerDocumenterUtility.App_Start
{
    public static class AutoFacConfig
    {
        /// <summary>
        /// Configure AutoFac for usage in web api
        /// </summary>
        public static void ConfigureWebApi()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            //Pull in module built in Data project
            builder.RegisterModule(new DataIocModule());

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}