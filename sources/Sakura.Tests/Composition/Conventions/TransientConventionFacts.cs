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

    public class TransientConventionFacts
    {
        private readonly ContainerBuilder containerBuilder;

        private readonly TransientConvention convention;

        public TransientConventionFacts()
        {
            this.convention = new TransientConvention();
            this.containerBuilder = new ContainerBuilder();
        }

        [Fact]
        public void should_match_to_transient_dependency()
        {
            var shouldMatch = this.convention.IsMatch(typeof(MockTransientDependency));

            shouldMatch.Should().BeTrue();
        }

        [Fact]
        public void should_register_as_transient()
        {
            this.convention.Apply(typeof(MockTransientDependency), this.containerBuilder);

            var container = this.containerBuilder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockTransientDependency))).
                    SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }
    }
}