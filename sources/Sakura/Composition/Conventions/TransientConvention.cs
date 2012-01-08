namespace Sakura.Composition.Conventions
{
    using System;
    using System.Linq;

    using Autofac;

    using Sakura.Bootstrapping;
    using Sakura.Composition.Markers;
    using Sakura.ExtensionMethods;

    public class TransientConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            foreach (var itf in dependencyType.GetInterfaces().Where(i => i.HasInterface(typeof(ITransientDependency))))
            {
                builder.RegisterType(dependencyType).As(itf).InstancePerDependency();
            }
        }

        public bool IsMatch(Type type)
        {
            return typeof(ITransientDependency).IsAssignableFrom(type);
        }
    }
}