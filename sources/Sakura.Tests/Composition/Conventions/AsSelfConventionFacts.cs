namespace Sakura.Framework.Tests.Composition.Conventions
{
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using Sakura.Composition.Conventions;
    using Sakura.Framework.Tests.StaticMocks;

    using Xunit;

    public class AsSelfConventionFacts
    {
        private readonly ContainerBuilder containerBuilder;

        private readonly AsSelfConvention convention;

        public AsSelfConventionFacts()
        {
            this.convention = new AsSelfConvention();
            this.containerBuilder = new ContainerBuilder();
        }

        [Fact]
        public void should_match_to_as_self()
        {
            var shouldMatch = this.convention.IsMatch(typeof(MockTransientAsSelfDependency));

            shouldMatch.Should().BeTrue();
        }

        [Fact]
        public void should_register_as_transient()
        {
            var dependencyRegistration = this.containerBuilder.RegisterType<MockTransientAsSelfDependency>();
            this.convention.Apply(dependencyRegistration, typeof(MockTransientAsSelfDependency));

            var container = this.containerBuilder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(MockTransientAsSelfDependency))).
                    SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }
    }
}