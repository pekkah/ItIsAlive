namespace Sakura.Framework.Dependencies.Conventions
{
    using System;

    using Autofac;

    using Sakura.Bootstrapping;
    using Sakura.Framework.Dependencies.DefaultTypes;

    public class AsSelfConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            var registration = builder.RegisterType(dependencyType).AsSelf();

            if (typeof(ISingleInstanceDependency).IsAssignableFrom(dependencyType))
            {
                registration.SingleInstance();
            }
            else if (typeof(ITransientDependency).IsAssignableFrom(dependencyType))
            {
                registration.InstancePerDependency();
            }
        }

        public bool IsMatch(Type type)
        {
            return typeof(IAsSelf).IsAssignableFrom(type);
        }
    }
}