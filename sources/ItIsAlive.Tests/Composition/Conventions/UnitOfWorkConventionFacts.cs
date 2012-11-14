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

    public class UnitOfWorkConventionFacts
    {
        private readonly ContainerBuilder containerBuilder;

        private readonly UnitOfWorkConvention convention;

        public UnitOfWorkConventionFacts()
        {
            convention = new UnitOfWorkConvention();
            containerBuilder = new ContainerBuilder();
        }

        [Fact]
        public void should_match_to_unitOfWork_dependency()
        {
            var shouldMatch = convention.IsMatch(typeof(MockUnitOfWorkDependency));

            shouldMatch.Should().BeTrue();
        }

        [Fact]
        public void should_register_as_scoped_to_unitOfWork()
        {
            var dependencyRegistration = containerBuilder.RegisterType<MockUnitOfWorkDependency>();
            convention.Apply(dependencyRegistration, typeof(MockUnitOfWorkDependency));

            var container = containerBuilder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockUnitOfWorkDependency))).
                          SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<MatchingScopeLifetime>();
        }
    }
}