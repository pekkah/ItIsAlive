namespace Sakura.Framework.Registration
{
    using System;
    using System.Linq;

    using Autofac;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.ExtensionMethods;

    public class SingleInstancePolicy : IRegistrationPolicy
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            foreach (var itf in dependencyType.GetInterfaces().Where(i => i.HasInterface(typeof(ISingleInstanceDependency))))
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