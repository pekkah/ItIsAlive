namespace ItIsAlive.Framework.Tests.Composition.Conventions
{
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using ItIsAlive.Composition.Conventions;

    using FluentAssertions;

    using StaticMocks;

    using Xunit;

    public class SingleInstanceConventionFacts
    {
        private readonly ContainerBuilder containerBuilder;

        private readonly SingleInstanceConvention convention;

        public SingleInstanceConventionFacts()
        {
            convention = new SingleInstanceConvention();
            containerBuilder = new ContainerBuilder();
        }

        [Fact]
        public void should_match_to_singleInstance_dependency()
        {
            var shouldMatch = convention.IsMatch(typeof(MockSingleInstanceDependency));

            shouldMatch.Should().BeTrue();
        }

        [Fact]
        public void should_register_as_single_instance()
        {
            var dependencyRegistration = containerBuilder.RegisterType<MockSingleInstanceDependency>();
            convention.Apply(dependencyRegistration, typeof(MockSingleInstanceDependency));

            var container = containerBuilder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockSingleInstanceDependency))).
                          SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<RootScopeLifetime>();
        }
    }
}