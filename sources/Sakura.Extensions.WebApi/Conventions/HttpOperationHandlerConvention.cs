namespace Sakura.Extensions.WebApi.Conventions
{
    using System;

    using Autofac.Builder;

    using Microsoft.ApplicationServer.Http.Dispatcher;

    using Sakura.Composition;
    using Sakura.ExtensionMethods;

    public class HttpOperationHandlerConvention : IRegistrationConvention
    {
        public void Apply(
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration,
            Type dependencyType)
        {
            registration.As<HttpOperationHandler>().InstancePerLifetimeScope();
            registration.ApplyPriority(dependencyType);
        }

        public bool IsMatch(Type type)
        {
            if (type.IsAbstract)
            {
                return false;
            }

            return typeof(HttpOperationHandler).IsAssignableFrom(type);
        }
    }
}