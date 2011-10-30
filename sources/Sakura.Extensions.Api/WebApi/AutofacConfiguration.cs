namespace Fugu.Extensions.Api.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.ServiceModel;

    using Autofac;

    using Microsoft.ApplicationServer.Http;

    public class ApiConfiguration : WebApiConfiguration
    {
        public static readonly string WebApiRequestToken = "WebApiRequest";

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
            var lifetime = this.container.BeginLifetimeScope(WebApiRequestToken);

            var lifetimeExtension = new AutofacLifetimeExtension(lifetime);

            context.Extensions.Add(lifetimeExtension);

            return lifetime.Resolve(serviceType);
        }
    }

    public class AutofacLifetimeExtension : IExtension<InstanceContext>, IDisposable
    {
        private readonly ILifetimeScope lifetimeScope;

        private bool isDisposed;

        public AutofacLifetimeExtension(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }

            this.lifetimeScope.Dispose();
            this.isDisposed = true;
        }

        public void Attach(InstanceContext owner)
        {
        }

        public void Detach(InstanceContext owner)
        {
            this.Dispose();
        }
    }
}