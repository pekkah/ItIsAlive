namespace Sakura.Extensions.Web.WebApi.Conventions
{
    using System;
    using System.Net.Http;

    using Autofac.Builder;

    using Sakura.Composition;
    using Sakura.ExtensionMethods;

    public class DelegatingHandlerConvention : IRegistrationConvention
    {
        public void Apply(
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration,
            Type dependencyType)
        {
            registration.As<DelegatingHandler>().InstancePerLifetimeScope();

            registration.ApplyPriority(dependencyType);
        }

        public bool IsMatch(Type type)
        {
            return typeof(DelegatingHandler).IsAssignableFrom(type);
        }
    }
}