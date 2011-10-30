namespace Sakura.Extensions.Data.Tests
{
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using NHibernate;

    using NUnit.Framework;

    using Sakura.Extensions.Data;
    using Sakura.Framework;

    [TestFixture]
    public class When_registered_as_dependency
    {
        private IContainer container;

        private Bootstrapper bootstrapper;

        [SetUp]
        public void Setup()
        {
            this.bootstrapper = new SetupBoot()
                .DependenciesFrom(typeof(RegisterNHibernate))
                .ExposeContainer(exposed => this.container = exposed)
                .Start();
        }

        [Test]
        public void should_register_session()
        {
            var registration =
                this.container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(ISession))).SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }
    }
}