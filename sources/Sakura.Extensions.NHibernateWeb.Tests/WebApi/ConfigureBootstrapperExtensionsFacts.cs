namespace Sakura.Extensions.NHibernateWeb.Tests.WebApi
{
    using System.Collections.Generic;
    using System.Net.Http;

    using Autofac;

    using FluentAssertions;

    using Microsoft.ApplicationServer.Http.Dispatcher;

    using Sakura.Bootstrapping;
    using Sakura.Extensions.NHibernateWeb.WebApi;
    using Sakura.Extensions.Web.WebApi;

    using Xunit;

    public class ConfigureBootstrapperExtensionsFacts
    {
        [Fact]
        public void should_register_unitOfWork_OperationHandler()
        {
            IContainer container = null;
            var bootstrapper = new Configure()
                .ConfigureWebApi(factory => {})
                .EnableWebApiUnitOfWork()
                .ExposeContainer(exposed => container = exposed)
                .Start();

            var operationHanders = container.Resolve<IEnumerable<HttpOperationHandler>>();

            operationHanders.Should().Contain(handler => handler.GetType() == typeof(UnitOfWorkOperationHandler));
        }

        [Fact]
        public void should_register_unitOfWork_DelegatingHandler()
        {
            IContainer container = null;
            var bootstrapper = new Configure()
                .ConfigureWebApi(factory =>
                {
                })
                .EnableWebApiUnitOfWork()
                .ExposeContainer(exposed => container = exposed)
                .Start();

            var delegatingHandlers = container.Resolve<IEnumerable<DelegatingHandler>>();

            delegatingHandlers.Should().Contain(handler => handler.GetType() == typeof(UnitOfWorkDelegatingHandler));
        }
    }
}