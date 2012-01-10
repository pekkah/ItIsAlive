namespace Sakura.Composition.Conventions
{
    using System;
    using System.Linq;

    using Autofac.Builder;

    using Sakura.Composition.Markers;
    using Sakura.ExtensionMethods;

    public class UnitOfWorkConvention : IRegistrationConvention
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

            foreach (var itf in dependencyType.GetInterfaces().Where(i => i.HasInterface(typeof(IUnitOfWorkDependency)))
                )
            {
                registration.As(itf).InstancePerMatchingLifetimeScope("unitOfWork");
            }
        }

        public bool IsMatch(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsAbstract && type.IsInterface)
            {
                return false;
            }

            return typeof(IUnitOfWorkDependency).IsAssignableFrom(type);
        }
    }
}