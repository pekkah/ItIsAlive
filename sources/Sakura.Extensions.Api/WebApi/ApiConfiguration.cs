namespace Sakura.Extensions.Api.WebApi
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.ServiceModel;
    using System.ServiceModel.Description;

    using Autofac;

    using Microsoft.ApplicationServer.Http;
    using Microsoft.ApplicationServer.Http.Description;
    using Microsoft.ApplicationServer.Http.Dispatcher;

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

        private void GetRequestHandlers(Collection<HttpOperationHandler> handlers, ServiceEndpoint endpoint, HttpOperationDescription description)
        {
            foreach (var input in description.InputParameters)
            {
                var returnValueType = input.ParameterType;

                var handlerType = typeof(HttpOperationHandler<,>).MakeGenericType(
                    typeof(HttpRequestMessage), returnValueType);

                var resolvedHandlers = this.container.Resolve(typeof(IEnumerable<>).MakeGenericType(handlerType)) as IEnumerable<HttpOperationHandler>;

                foreach (var resolvedHandler in resolvedHandlers)
                {
                    handlers.Add(resolvedHandler);
                }
            }
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