using Nancy.Bootstrappers.Autofac;
using Autofac;
using SqlServerDocumenterUtility.Data;

namespace SqlServerDocumenterUtility.NancyApi
{
    /// <summary>
    /// Nancy Bootstrapper to configure Autofac IoC
    /// </summary>
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            //Pull in module built in Data project
            container.Update(builder=> builder.RegisterModule(new DataIocModule()));
            base.ConfigureApplicationContainer(container);
        }

    }
}