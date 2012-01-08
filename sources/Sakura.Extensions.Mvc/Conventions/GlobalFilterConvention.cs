namespace Sakura.Extensions.Mvc.Conventions
{
    using System;

    using Autofac;

    using Sakura.Bootstrapping;
    using Sakura.Composition;

    public class GlobalFilterConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            var registration =
                builder.RegisterType(dependencyType).As<IGlobalFilter>().InstancePerLifetimeScope().PropertiesAutowired(
                    );

            RegistrationHelper.ApplyPriority(dependencyType, registration);
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