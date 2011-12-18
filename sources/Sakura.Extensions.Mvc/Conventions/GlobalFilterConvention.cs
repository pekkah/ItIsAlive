namespace Sakura.Extensions.Mvc.Conventions
{
    using System;

    using Autofac;
    using Autofac.Builder;

    using Sakura.Bootstrapping;
    using Sakura.Framework.Dependencies.Discovery;

    public class GlobalFilterConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            var registration =
                builder.RegisterType(dependencyType).As<IGlobalFilter>().InstancePerLifetimeScope().PropertiesAutowired(
                    );

            this.ApplyPriority(dependencyType, registration);
        }

        public bool IsMatch(Type type)
        {
            if (type.IsAbstract || !type.IsClass)
            {
                return false;
            }

            return typeof(IGlobalFilter).IsAssignableFrom(type);
        }

        private void ApplyPriority(
            Type dependencyType, 
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration)
        {
            // todo move the containing method into a helper
            var priorityAttribute =
                (PriorityAttribute)Attribute.GetCustomAttribute(dependencyType, typeof(PriorityAttribute));

            // default priority is
            var priority = 100;

            if (priorityAttribute != null)
            {
                priority = priorityAttribute.Priority;
            }

            registration.WithMetadata<IPriorityMetadata>(configure => configure.For(meta => meta.Priority, priority));
        }
    }
}