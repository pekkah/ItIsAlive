namespace Sakura.Extensions.Mvc.Policies
{
    using System;

    using Autofac;

    using Sakura.Framework.Dependencies.Policies;

    public class GlobalFilterPolicy : IRegistrationPolicy
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            builder.RegisterType(dependencyType).As<IGlobalFilter>().InstancePerDependency().PropertiesAutowired();
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