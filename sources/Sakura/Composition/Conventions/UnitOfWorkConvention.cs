namespace Sakura.Composition.Conventions
{
    using System;
    using System.Linq;

    using Autofac;

    using Sakura.Bootstrapping;
    using Sakura.Composition.Markers;
    using Sakura.ExtensionMethods;

    public class UnitOfWorkConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            foreach (var itf in dependencyType.GetInterfaces().Where(i => i.HasInterface(typeof(IUnitOfWorkDependency))))
            {
                builder.RegisterType(dependencyType).As(itf).InstancePerMatchingLifetimeScope("unitOfWork");
            }
        }

        public bool IsMatch(Type type)
        {
            if (type.IsAbstract && type.IsInterface)
            {
                return false;
            }

            return typeof(IUnitOfWorkDependency).IsAssignableFrom(type);
        }
    }
}