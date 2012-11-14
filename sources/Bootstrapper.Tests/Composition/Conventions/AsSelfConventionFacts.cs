namespace Bootstrapper.Framework.Tests.Composition.Conventions
{
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using Bootstrapper.Composition.Conventions;

    using FluentAssertions;

    using StaticMocks;

    using Xunit;

    public class AsSelfConventionFacts
    {
        private readonly ContainerBuilder containerBuilder;

        private readonly AsSelfConvention convention;

        public AsSelfConventionFacts()
        {
            convention = new AsSelfConvention();
            containerBuilder = new ContainerBuilder();
        }

        [Fact]
        public void should_match_to_as_self()
        {
            var shouldMatch = convention.IsMatch(typeof(MockTransientAsSelfDependency));

            shouldMatch.Should().BeTrue();
        }

        [Fact]
        public void should_register_as_transient()
        {
            var dependencyRegistration = containerBuilder.RegisterType<MockTransientAsSelfDependency>();
            convention.Apply(dependencyRegistration, typeof(MockTransientAsSelfDependency));

            var container = containerBuilder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(MockTransientAsSelfDependency))).
                          SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }
    }
}