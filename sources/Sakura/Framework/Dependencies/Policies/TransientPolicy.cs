namespace Sakura.Framework.Dependencies.Policies
{
    using System;
    using System.Linq;

    using Autofac;

    using Sakura.ExtensionMethods;
    using Sakura.Framework.Dependencies.DefaultTypes;

    public class TransientPolicy : IRegistrationPolicy
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