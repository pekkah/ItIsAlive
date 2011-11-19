namespace Sakura.Framework.Dependencies.Conventions
{
    using System;
    using System.Linq;

    using Autofac;

    using Sakura.ExtensionMethods;
    using Sakura.Framework.Dependencies.DefaultTypes;

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