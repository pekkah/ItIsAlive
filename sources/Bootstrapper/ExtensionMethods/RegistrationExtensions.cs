namespace Bootstrapper.ExtensionMethods
{
    using System;

    using Autofac.Builder;

    using Composition.Discovery;

    public static class RegistrationExtensions
    {
        public static void ApplyPriority(
            this IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration,
            Type dependencyType)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            if (dependencyType == null)
            {
                throw new ArgumentNullException("dependencyType");
            }

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