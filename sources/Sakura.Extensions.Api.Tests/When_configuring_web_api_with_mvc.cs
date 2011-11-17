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
    using Sakura.Extensions.Api.WebApi;
    using Sakura.Extensions.Mvc;
    using Sakura.Framework;

    [TestFixture]
    public class When_configuring_web_api_with_mvc
    {
        private Bootstrapper bootstrapper;

        private IContainer container;

        [SetUp]
        public void Setup()
        {
            this.bootstrapper = new Setup()
                .Dependencies(dependencies => dependencies.AssemblyOf<PersonApi>())
                .ConfigureMvc(() => { })
                .ConfigureWebApi(configurationFactory => { })
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