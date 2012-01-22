namespace Sakura.Extensions.NHibernateWeb.Tests.WebApi
{
    using System;
    using System.Net.Http;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Autofac;

    using FluentAssertions;

    using Microsoft.ApplicationServer.Http;

    using NSubstitute;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Tasks;
    using Sakura.Extensions.NHibernate;
    using Sakura.Extensions.NHibernateWeb.WebApi;
    using Sakura.Extensions.Web.WebApi;

    using Xunit;

    using global::NHibernate;
    using global::NHibernate.Cfg;
    using global::NHibernate.Dialect;
    using global::NHibernate.Driver;

    using Environment = global::NHibernate.Cfg.Environment;

    public class UnitOfWorkIntegrationFacts : IDisposable
    {
        private ApiConfiguration configuration;

        private readonly Bootstrapper bootstrapper;

        private readonly ISession unitOfWork;

        private readonly ITransaction transaction;

        private const string ServiceBaseAddress = "http://localhost/tests";

        public UnitOfWorkIntegrationFacts()
        {
            this.unitOfWork = Substitute.For<ISession>();
            this.transaction = Substitute.For<ITransaction>();
            this.unitOfWork.Transaction.Returns(this.transaction);

            this.configuration = null;

            this.bootstrapper =
                new Configure().Dependencies(from => from.Types(typeof(Resource))).ConfigureWebApi(
                    factory =>
                    {
                        this.configuration = factory();
                    })
                    .EnableWebApiUnitOfWork()
                    .Tasks(tasks => tasks.Add(
                        new ActionTask(context => context.Builder
                            .RegisterInstance(this.unitOfWork)
                            .AsImplementedInterfaces())))
                    .Start();
        }

        [Fact]
        public void should_begin_and_commit_unitOfWork()
        {
            this.transaction.IsActive.Returns(true);

            using (var host = new HttpServiceHost(typeof(Resource), configuration, ServiceBaseAddress))
            {
                host.Open();

                var client = new HttpClient();

                var result = client.GetAsync(ServiceBaseAddress + "/use").Result;

                result.IsSuccessStatusCode.Should().BeTrue();
                this.unitOfWork.Received().BeginTransaction();
                this.transaction.Received().Commit();
            }
        }

        [Fact]
        public void should_begin_and_rollback_unitOfWork_on_failed_request()
        {
            this.transaction.IsActive.Returns(true);

            using (var host = new HttpServiceHost(typeof(Resource), configuration, ServiceBaseAddress))
            {
                host.Open();

                var client = new HttpClient();

                var result = client.GetAsync(ServiceBaseAddress + "/fail").Result;

                result.IsSuccessStatusCode.Should().BeFalse();
                this.unitOfWork.Received().BeginTransaction();
                this.transaction.Received().Rollback();
            }
        }

        [Fact]
        public void should_not_begin_when_not_used_as_parameter()
        {
            using (var host = new HttpServiceHost(typeof(Resource), configuration, ServiceBaseAddress))
            {
                host.Open();

                var client = new HttpClient();

                var result = client.GetAsync(ServiceBaseAddress + "/do-not-use").Result;

                result.IsSuccessStatusCode.Should().BeTrue();
                this.unitOfWork.DidNotReceive().BeginTransaction();
                this.transaction.DidNotReceive().Rollback();
                this.transaction.DidNotReceive().Commit();
            }
        }

        public void Dispose()
        {
            this.bootstrapper.Shutdown();
        }
    }

    [ServiceContract]
    public class Resource
    {
        [WebGet(UriTemplate = "use")]
        public HttpResponseMessage UseParameter(ISession session)
        {
            return new HttpResponseMessage();
        }

        [WebGet(UriTemplate = "fail")]
        public HttpResponseMessage Fail(ISession session)
        {
            throw new HttpRequestException("error");
        }

        [WebGet(UriTemplate = "do-not-use")]
        public HttpResponseMessage DoNotUse()
        {
            return new HttpResponseMessage();
        }
    }
}