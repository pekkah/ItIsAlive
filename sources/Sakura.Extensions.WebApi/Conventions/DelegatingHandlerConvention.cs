namespace Sakura.Extensions.WebApi.Conventions
{
    using System;
    using System.Net.Http;

    using Autofac;

    using Sakura.Framework.Dependencies.Conventions;

    public class DelegatingHandlerConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            builder.RegisterType(dependencyType).As<DelegatingHandler>().InstancePerLifetimeScope();
        }

        public bool IsMatch(Type type)
        {
            return typeof(DelegatingHandler).IsAssignableFrom(type);
        }
    }
}