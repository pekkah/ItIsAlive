namespace Sakura.Composition.Conventions
{
    using System;
    using System.Linq;

    using Autofac;

    using Sakura.Composition.Markers;
    using Sakura.ExtensionMethods;

    public class SingleInstanceConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            foreach (
                var itf in dependencyType.GetInterfaces().Where(i => i.HasInterface(typeof(ISingleInstanceDependency))))
            {
                builder.RegisterType(dependencyType).As(itf).SingleInstance();
            }
        }

        public bool IsMatch(Type type)
        {
            return typeof(ISingleInstanceDependency).IsAssignableFrom(type);
        }
    }
}