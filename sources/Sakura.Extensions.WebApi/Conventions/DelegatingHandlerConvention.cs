namespace Sakura.Extensions.WebApi.Conventions
{
    using System;
    using System.Net.Http;

    using Autofac;

    using Sakura.Bootstrapping;
    using Sakura.Extensions.Mvc.Conventions;

    public class DelegatingHandlerConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            var registration = builder.RegisterType(dependencyType).As<DelegatingHandler>().InstancePerLifetimeScope();

            RegistrationHelper.ApplyPriority(dependencyType, registration);
        }

        public bool IsMatch(Type type)
        {
            return typeof(DelegatingHandler).IsAssignableFrom(type);
        }
    }
}