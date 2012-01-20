namespace Sakura.Extensions.NHibernateWeb.Tests.WebApi
{
    using System;
    using System.Net.Http;

    using Autofac;

    using NSubstitute;

    using Sakura.Extensions.NHibernate;
    using Sakura.Extensions.NHibernateWeb.WebApi;

    using Xunit;

    public class WebApiUnitOfWorkHandlerFacts : IDisposable
    {
        private readonly IContainer container;

        private readonly IUnitOfWork unitOfWork;

        public WebApiUnitOfWorkHandlerFacts()
        {
            this.unitOfWork = Substitute.For<IUnitOfWork>();
            this.unitOfWork.IsActive.Returns(true);

            var builder = new ContainerBuilder();
            builder.RegisterInstance(this.unitOfWork).As<IUnitOfWork>();

            this.container = builder.Build();
        }

        public void Dispose()
        {
            this.container.Dispose();
        }

        [Fact]
        public void should_begin_unitOfWork()
        {
            var handler = new WebApiUnitOfWorkHandler();
            var request = new HttpRequestMessage();
            request.Properties.Add("unitOfWorkScope", this.container.BeginLifetimeScope("unitOfWork"));

            handler.BeginIfRequired(request);

            this.unitOfWork.Received().Begin();
        }

        [Fact]
        public void should_commit_unitOfWork()
        {
            var handler = new WebApiUnitOfWorkHandler();
            var response = new HttpResponseMessage() { RequestMessage = new HttpRequestMessage() };
            response.RequestMessage.Properties.Add("unitOfWorkScope", this.container.BeginLifetimeScope("unitOfWork"));
       
            handler.EndIfRequired(response, null);

            this.unitOfWork.Received().Commit();
        }
    }
}