namespace Sakura.Framework.Registration
{
    using System;
    using System.Linq;

    using Autofac;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.ExtensionMethods;

    public class TransientPolicy : IRegistrationPolicy
    {
        public bool IsMatch(Type type)
        {
            return typeof(ITransientDependency).IsAssignableFrom(type);
        }

        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            foreach (var itf in dependencyType.GetInterfaces().Where(i => i.HasInterface(typeof(ITransientDependency))))
            {
                builder.RegisterType(dependencyType).As(itf).InstancePerDependency();
            }
        }
    }
}