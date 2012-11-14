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

    public class TransientConventionFacts
    {
        private readonly ContainerBuilder containerBuilder;

        private readonly TransientConvention convention;

        public TransientConventionFacts()
        {
            convention = new TransientConvention();
            containerBuilder = new ContainerBuilder();
        }

        [Fact]
        public void should_match_to_transient_dependency()
        {
            var shouldMatch = convention.IsMatch(typeof(MockTransientDependency));

            shouldMatch.Should().BeTrue();
        }

        [Fact]
        public void should_register_as_transient()
        {
            var dependencyRegistration = containerBuilder.RegisterType<MockTransientDependency>();
            convention.Apply(dependencyRegistration, typeof(MockTransientDependency));

            var container = containerBuilder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockTransientDependency))).
                          SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }
    }
}