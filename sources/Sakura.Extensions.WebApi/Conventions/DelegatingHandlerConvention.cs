namespace Sakura.Extensions.WebApi.Conventions
{
    using System;
    using System.Net.Http;

    using Autofac;

    using Sakura.Bootstrapping;
    using Sakura.Composition;
    using Sakura.ExtensionMethods;

    public class DelegatingHandlerConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            var registration = builder
                .RegisterType(dependencyType)
                .As<DelegatingHandler>()
                .InstancePerLifetimeScope();

            registration.ApplyPriority(dependencyType);
        }

        public bool IsMatch(Type type)
        {
            return typeof(DelegatingHandler).IsAssignableFrom(type);
        }
    }
}