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

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Setup;
    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Extensions.Api.Tests.Apis;

    [TestFixture]
    public class When_configuring_web_api
    {
        private Bootstrapper bootstrapper;

        private IContainer container;

        [SetUp]
        public void Setup()
        {
            var initializeTest = Substitute.For<IInitializationTask>();

            this.bootstrapper = new Setup()
                .ConfigureWebApi(configurationFactory => { })
                .Tasks(manager => manager.AddTask(initializeTest))
                .Dependencies(dependencies => dependencies.Types(typeof(PersonApi)))
                .ExposeContainer(exposed => this.container = exposed)
                .Start();
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