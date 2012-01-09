namespace Sakura.Extensions.Mvc.Conventions
{
    using System;

    using Autofac.Builder;

    using Sakura.Composition;
    using Sakura.ExtensionMethods;

    public class GlobalFilterConvention : IRegistrationConvention
    {
        public void Apply(
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration,
            Type dependencyType)
        {
            registration.As<IGlobalFilter>().InstancePerLifetimeScope().PropertiesAutowired();
            registration.ApplyPriority(dependencyType);
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