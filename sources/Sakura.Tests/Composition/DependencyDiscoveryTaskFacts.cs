namespace Sakura.Framework.Tests.Composition
{
    using System;
    using System.Linq;

    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;

    using FluentAssertions;

    using NSubstitute;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Composition;
    using Sakura.Composition.Discovery;
    using Sakura.Framework.Tests.StaticMocks;

    using Xunit;

    public class DependencyDiscoveryTaskFacts
    {
        private readonly DependencyDiscoveryTask discoveryTask;

        public DependencyDiscoveryTaskFacts()
        {
            var locator = new ListLocator(typeof(MockDependency));
            this.discoveryTask = new DependencyDiscoveryTask(locator);
        }

        [Fact]
        public void should_add_convention()
        {
            var convention = Substitute.For<IRegistrationConvention>();
            this.discoveryTask.AddConvention(convention);
            this.discoveryTask.Conventions.Should().Contain(convention);
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
                                    ci.Arg<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>();
                                
                                dpr.As<IMockDependency>();
                            });

            // act
            this.discoveryTask.AddConvention(convention);
            this.discoveryTask.Execute(context);

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
            this.discoveryTask.AddConvention(convention);

            this.discoveryTask.RemoveConvention(convention);

            this.discoveryTask.Conventions.Should().NotContain(convention);
        }
    }
}