namespace Fugu.Extensions.Api.Tests
{
    using System.Linq;
    using System.Web.Routing;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using Fugu.Extensions.Api.Apis;
    using Fugu.Extensions.Api.WebApi;
    using Fugu.Framework;
    using Fugu.Framework.Tasks;

    using NSubstitute;

    using NUnit.Framework;

    [TestFixture]
    public class When_register_as_dependency
    {
        private IContainer container;

        private Bootstrapper bootstrapper;

        [SetUp]
        public void Setup()
        {
            var initializeTest = Substitute.For<IInitializationTask>();
            var routes = Substitute.For<IRouting>();

            initializeTest.Execute(Arg.Do<InitializationTaskContext>(c => c.Builder.RegisterInstance(routes).AsImplementedInterfaces()));

            this.bootstrapper = new SetupBoot()
                .DependenciesFrom(typeof(StartApi))
                .Task(initializeTest)
                .ExposeContainer(exposed => this.container = exposed)
                .Start();
        }

        [Test]
        public void should_register_projects_api()
        {
            var registration = this.container
                .ComponentRegistry
                .RegistrationsFor(new TypedService(typeof(ProjectsApi)))
                .SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }

        [Test]
        public void should_register_http_dependencies()
        {
            var registration = this.container
                .ComponentRegistry
                .RegistrationsFor(new TypedService(typeof(RouteCollection)))
                .SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<RootScopeLifetime>();
        }
    }
}