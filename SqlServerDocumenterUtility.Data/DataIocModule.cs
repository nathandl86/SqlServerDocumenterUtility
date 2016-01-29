using Autofac;
using System;
using System.Reflection;

namespace SqlServerDocumenterUtility.Data
{
    public class DataIocModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            
            builder.RegisterType<DalHelper>().AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Dal") || t.Name.EndsWith("Mapper"))
                .AsImplementedInterfaces();            
        }

    }
}
