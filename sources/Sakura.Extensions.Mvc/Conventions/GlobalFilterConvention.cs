namespace Sakura.Extensions.Mvc.Conventions
{
    using System;

    using Autofac;

    using Sakura.Framework.Dependencies.Conventions;

    public class GlobalFilterConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            builder.RegisterType(dependencyType)
                .As<IGlobalFilter>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }

        public bool IsMatch(Type type)
        {
            if (type.IsAbstract || !type.IsClass)
            {
                return false;
            }

            return typeof(IGlobalFilter).IsAssignableFrom(type);
        }
    }
}