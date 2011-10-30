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

    using Sakura.Extensions.Api.WebApi;
    using Sakura.Framework;
    using Sakura.Framework.Tasks;

    [TestFixture]
    public class When_registered_as_dependency
    {
        private Bootstrapper bootstrapper;

        private IContainer container;

        [SetUp]
        public void Setup()
        {
            var initializeTest = Substitute.For<IInitializationTask>();
            var routes = Substitute.For<IRouting>();

            initializeTest.Execute(
                Arg.Do<InitializationTaskContext>(c => c.Builder.RegisterInstance(routes).AsImplementedInterfaces()));

            this.bootstrapper =
                new SetupBoot().DependenciesFrom(typeof(InitializeServiceDependencies)).Task(initializeTest).ExposeContainer(
                    exposed => this.container = exposed).Start();
        }

        [Test]
        public void should_register_http_dependencies()
        {
            var registration =
                this.container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(RouteCollection))).
                    SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<RootScopeLifetime>();
        }


    }
}