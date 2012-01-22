namespace Sakura.Extensions.Web.WebApi
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.ServiceModel;
    using System.ServiceModel.Description;

    using Autofac;

    using Microsoft.ApplicationServer.Http;
    using Microsoft.ApplicationServer.Http.Description;
    using Microsoft.ApplicationServer.Http.Dispatcher;

    using Sakura.Composition.Discovery;

    public class ApiConfiguration : WebApiConfiguration
    {
        private readonly ILifetimeScope container;

        public ApiConfiguration(ILifetimeScope container)
        {
            this.container = container;

            this.CreateInstance = this.OncreateInstance;

            this.MessageHandlerFactory = this.OnCreateMessageHandlers;

            this.RequestHandlers = this.GetRequestHandlers;
        }

        private void GetRequestHandlers(
            Collection<HttpOperationHandler> handlers, ServiceEndpoint endpoint, HttpOperationDescription description)
        {
            var registeredHandlers = this.container
                .Resolve<IEnumerable<Lazy<HttpOperationHandler, IPriorityMetadata>>>();

            foreach (var lazyHandler in registeredHandlers.OrderBy(h => h.Metadata.Priority))
            {
                var handler = lazyHandler.Value;


                // check if applies to operation if IAppliesTo present
                var applies = handler as IApplies<HttpOperationDescription>;
                if (applies != null)
                {
                    if (!applies.To(description))
                    {
                        // exclude from handlers
                        continue;
                    }
                }

                handlers.Add(handler);
            }
        }

        private IEnumerable<DelegatingHandler> OnCreateMessageHandlers()
        {
            var result =
                this.container.Resolve<IEnumerable<Lazy<DelegatingHandler, IPriorityMetadata>>>().OrderBy(
                    h => h.Metadata.Priority);

            return result.Select(h => h.Value);
        }

        private object OncreateInstance(Type serviceType, InstanceContext context, HttpRequestMessage message)
        {
            return this.container.Resolve(serviceType);
        }
    }
}