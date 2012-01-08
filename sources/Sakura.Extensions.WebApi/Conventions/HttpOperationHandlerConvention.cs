namespace Sakura.Extensions.WebApi.Conventions
{
    using System;

    using Autofac;

    using Microsoft.ApplicationServer.Http.Dispatcher;

    using Sakura.Bootstrapping;
    using Sakura.Composition;
    using Sakura.ExtensionMethods;

    public class HttpOperationHandlerConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            var registration = builder.RegisterType(dependencyType).As<HttpOperationHandler>().InstancePerLifetimeScope();
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