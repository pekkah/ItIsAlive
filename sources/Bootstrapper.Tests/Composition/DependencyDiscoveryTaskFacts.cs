namespace Bootstrapper.Framework.Tests.Composition
{
    using System;
    using System.Linq;

    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;

    using Bootstrapper.Bootstrapping.Tasks;
    using Bootstrapper.Composition;
    using Bootstrapper.Composition.Discovery;

    using FluentAssertions;

    using NSubstitute;

    using StaticMocks;

    using Xunit;

    public class DependencyDiscoveryTaskFacts
    {
        private readonly DependencyDiscoveryTask discoveryTask;

        public DependencyDiscoveryTaskFacts()
        {
            var locator = new ListLocator(typeof(MockDependency));
            discoveryTask = new DependencyDiscoveryTask(locator);
        }

        [Fact]
        public void should_add_convention()
        {
            var convention = Substitute.For<IRegistrationConvention>();
            discoveryTask.AddConvention(convention);
            discoveryTask.Conventions.Should().Contain(convention);
        }

        [Fact]
        public void should_discover_dependencies()
        {
            // setup
            var builder = new ContainerBuilder();
            var context = new InitializationTaskContext(builder);

            // discoverers IMockDependencies
            var convention = Substitute.For<IRegistrationConvention>();
            convention.IsMatch(Arg.Any<Type>()).Returns(true);

            convention.When(
                c =>
                c.Apply(
                    Arg.Any<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>(),
                    Arg.Any<Type>())).Do(
                        ci =>
                            {
                                var dpr =
                                    ci
                                        .Arg
                                        <
                                            IRegistrationBuilder
                                                <object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>();

                                dpr.As<IMockDependency>();
                            });

            // act
            discoveryTask.AddConvention(convention);
            discoveryTask.Execute(context);

            // assert
            var container = builder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockDependency))).
                          SingleOrDefault();

            registration.Should().NotBeNull();
        }

        [Fact]
        public void should_remove_convention()
        {
            var convention = Substitute.For<IRegistrationConvention>();
            discoveryTask.AddConvention(convention);

            discoveryTask.RemoveConvention(convention);

            discoveryTask.Conventions.Should().NotContain(convention);
        }
    }
}