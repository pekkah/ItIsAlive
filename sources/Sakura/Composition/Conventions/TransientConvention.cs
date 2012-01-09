namespace Sakura.Composition.Conventions
{
    using System;
    using System.Linq;

    using Autofac.Builder;

    using Sakura.Composition.Markers;
    using Sakura.ExtensionMethods;

    public class TransientConvention : IRegistrationConvention
    {
        public void Apply(
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration,
            Type dependencyType)
        {
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