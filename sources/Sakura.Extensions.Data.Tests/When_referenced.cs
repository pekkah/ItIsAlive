namespace Fugu.Extensions.Data.Tests
{
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using Fugu.Framework;

    using NHibernate;

    using NUnit.Framework;

    [TestFixture]
    public class When_register_as_dependency
    {
        private IContainer container;

        private Bootstrapper bootstrapper;

        [SetUp]
        public void Setup()
        {
            this.bootstrapper = new SetupBoot()
                .DependenciesFrom(typeof(SessionFactoryFactory))
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

        [Test]
        public void should_register_session_factory()
        {
            var registration =
                this.container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(ISessionFactory))).SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<RootScopeLifetime>();
        }
    }
}