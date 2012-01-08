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

    public class SingleInstanceConventionFacts
    {
        private readonly ContainerBuilder containerBuilder;

        private readonly SingleInstanceConvention convention;

        public SingleInstanceConventionFacts()
        {
            this.convention = new SingleInstanceConvention();
            this.containerBuilder = new ContainerBuilder();
        }

        [Fact]
        public void should_match_to_singleInstance_dependency()
        {
            var shouldMatch = this.convention.IsMatch(typeof(MockSingleInstanceDependency));

            shouldMatch.Should().BeTrue();
        }

        [Fact]
        public void should_register_as_single_instance()
        {
            this.convention.Apply(typeof(MockSingleInstanceDependency), this.containerBuilder);

            var container = this.containerBuilder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockSingleInstanceDependency))).
                    SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<RootScopeLifetime>();
        }
    }
}