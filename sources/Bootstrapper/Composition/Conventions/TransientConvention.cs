namespace Bootstrapper.Composition.Conventions
{
    using System;
    using System.Linq;

    using Autofac.Builder;

    using ExtensionMethods;

    using Markers;

    public class TransientConvention : IRegistrationConvention
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

            foreach (var itf in dependencyType.GetInterfaces().Where(i => i.HasInterface(typeof(ITransientDependency))))
            {
                registration.As(itf).InstancePerDependency();
            }
        }

        public bool IsMatch(Type type)
        {
            return typeof(ITransientDependency).IsAssignableFrom(type);
        }
    }
}