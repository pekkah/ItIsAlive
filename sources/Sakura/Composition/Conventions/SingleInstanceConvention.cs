namespace Sakura.Composition.Conventions
{
    using System;
    using System.Linq;

    using Autofac.Builder;

    using Sakura.Composition.Markers;
    using Sakura.ExtensionMethods;

    public class SingleInstanceConvention : IRegistrationConvention
    {
        public void Apply(
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration,
            Type dependencyType)
        {
            foreach (
                var itf in dependencyType.GetInterfaces().Where(i => i.HasInterface(typeof(ISingleInstanceDependency))))
            {
                registration.As(itf).SingleInstance();
            }
        }

        public bool IsMatch(Type type)
        {
            return typeof(ISingleInstanceDependency).IsAssignableFrom(type);
        }
    }
}