namespace Sakura.Extensions.Mvc.Conventions
{
    using System;

    using Autofac.Builder;

    using Sakura.Framework.Dependencies.Discovery;

    public class RegistrationHelper
    {
        public static void ApplyPriority(
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