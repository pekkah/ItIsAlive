namespace Sakura.Extensions.Api.Tests
{
    using System.Linq;
    using System.Web.Routing;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    using Sakura.Extensions.Api.Tests.Apis;
    using Sakura.Extensions.Api.WebApi;
    using Sakura.Extensions.Mvc;
    using Sakura.Framework;
    using Sakura.Framework.Fluent;
    using Sakura.Framework.Tasks;
    using Sakura.Framework.Tasks.Types;

    [TestFixture]
    public class When_configuring_web_api_with_mvc
    {
        private Bootstrapper bootstrapper;

        private IContainer container;

        [SetUp]
        public void Setup()
        {
            var initializeTest = Substitute.For<IInitializationTask>();
            var routes = Substitute.For<IWebApiRouter>();

            initializeTest.Execute(
                Arg.Do<InitializationTaskContext>(c => c.Builder.RegisterInstance(routes).AsImplementedInterfaces()));

            this.bootstrapper = new Setup()
                .Dependencies(d => d.AssemblyOf<PersonApi>())
                .Task(initializeTest)
                .ConfigureMvc(router => { })
                .ConfigureWebApi(
                    (router, config) =>
                        {
                            router.MapServiceRoute<PersonApi>("api/person", config);
                        }).
                ExposeContainer(exposed => this.container = exposed).Start();
        }

        [Test]
        public void should_register_http_dependencies()
        {
            var registration =
                this.container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(RouteCollection))).
                    SingleOrDefault();

            registration.Should().NotBeNull();
        }

        [Test]
        public void should_register_api()
        {
            var registration =
                this.container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(PersonApi))).SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }
    }
}