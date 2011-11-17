namespace Sakura.Extensions.Api.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.ServiceModel;

    using Autofac;

    using Microsoft.ApplicationServer.Http;

    public class ApiConfiguration : WebApiConfiguration
    {
        private readonly ILifetimeScope container;

        public ApiConfiguration(ILifetimeScope container)
        {
            this.container = container;

            this.CreateInstance = this.OncreateInstance;

            this.MessageHandlerFactory = this.OnCreateMessageHandlers;
        }

        private IEnumerable<DelegatingHandler> OnCreateMessageHandlers()
        {
            var result = this.container.Resolve<IEnumerable<DelegatingHandler>>();

            return result;
        }

        private object OncreateInstance(Type serviceType, InstanceContext context, HttpRequestMessage message)
        {
            return this.container.Resolve(serviceType);
        }
    }
}