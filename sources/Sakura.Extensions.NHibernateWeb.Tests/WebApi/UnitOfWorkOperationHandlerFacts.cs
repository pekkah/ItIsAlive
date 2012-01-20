namespace Sakura.Extensions.NHibernateWeb.Tests.WebApi
{
    using System;
    using System.Net.Http;

    using Autofac;

    using FluentAssertions;

    using NSubstitute;

    using Sakura.Extensions.NHibernate;
    using Sakura.Extensions.NHibernateWeb.WebApi;

    using Xunit;

    public class UnitOfWorkOperationHandlerFacts : IDisposable
    {
        private readonly IContainer container;

        private readonly IUnitOfWork unitOfWork;

        public UnitOfWorkOperationHandlerFacts()
        {
            this.unitOfWork = Substitute.For<IUnitOfWork>().As<IUnitOfWork>();
 
            var builder = new ContainerBuilder();
            builder.RegisterInstance(this.unitOfWork).AsImplementedInterfaces();
            this.container = builder.Build();
        }

        public void Dispose()
        {
            this.container.Dispose();
        }

        [Fact]
        public void should_begin_unitOfWork_lifetimescope()
        {
            var handler = new UnitOfWorkOperationHandler(this.container);
            var input = new HttpRequestMessage();

            handler.Handle(new object[] { input });

            var unitOfWorkScope = input.Properties["unitOfWorkScope"] as ILifetimeScope;

            unitOfWorkScope.Should().NotBeNull();
        }

        [Fact]
        public void should_begin_unitOfWork()
        {
            var handler = new UnitOfWorkOperationHandler(this.container);
            var input = new HttpRequestMessage();

            handler.Handle(new object[] { input });

            this.unitOfWork.Received().Begin();
        }
    }
}