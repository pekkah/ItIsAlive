namespace Sakura.Composition.Conventions
{
    using System;

    using Autofac;
    using Autofac.Builder;

    using Sakura.Composition.Markers;

    public class AsSelfConvention : IRegistrationConvention
    {
        public void Apply(
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration,
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

            registration.AsSelf();
        }

        public bool IsMatch(Type type)
        {
            return typeof(IAsSelf).IsAssignableFrom(type);
        }
    }
}