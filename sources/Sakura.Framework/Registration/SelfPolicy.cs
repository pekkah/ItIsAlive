namespace Sakura.Framework.Registration
{
    using System;

    using Autofac;

    using Sakura.Framework.Dependencies;

    public class SelfPolicy : IRegistrationPolicy
    {
        public bool IsMatch(Type type)
        {
            return typeof(IAsSelf).IsAssignableFrom(type);
        }

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
    }
}