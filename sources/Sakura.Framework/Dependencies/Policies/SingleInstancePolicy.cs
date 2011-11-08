namespace Sakura.Framework.Dependencies.Policies
{
    using System;
    using System.Linq;

    using Autofac;

    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.ExtensionMethods;

    public class SingleInstancePolicy : IRegistrationPolicy
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