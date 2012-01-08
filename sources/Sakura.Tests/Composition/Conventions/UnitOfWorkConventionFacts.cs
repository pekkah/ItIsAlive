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

    public class UnitOfWorkConventionFacts
    {
        private readonly ContainerBuilder containerBuilder;

        private readonly UnitOfWorkConvention convention;

        public UnitOfWorkConventionFacts()
        {
            this.convention = new UnitOfWorkConvention();
            this.containerBuilder = new ContainerBuilder();
        }

        [Fact]
        public void should_match_to_unitOfWork_dependency()
        {
            var shouldMatch = this.convention.IsMatch(typeof(MockUnitOfWorkDependency));

            shouldMatch.Should().BeTrue();
        }

        [Fact]
        public void should_register_as_scoped_to_unitOfWork()
        {
            this.convention.Apply(typeof(MockUnitOfWorkDependency), this.containerBuilder);

            var container = this.containerBuilder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockUnitOfWorkDependency))).
                    SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<MatchingScopeLifetime>();
        }
    }
}