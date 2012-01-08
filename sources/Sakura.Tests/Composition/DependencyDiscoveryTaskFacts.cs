namespace Sakura.Framework.Tests.Composition
{
    using System;
    using System.Linq;

    using Autofac;
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
            var locator = new ListLocator(typeof(IMockTransientDependency));
            this.discoveryTask = new DependencyDiscoveryTask(locator);
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
            convention.IsMatch(Arg.Is<Type>(type => typeof(IMockTransientDependency).IsAssignableFrom(type))).Returns(true);

            convention.When(c => c.Apply(Arg.Any<Type>(), Arg.Any<ContainerBuilder>())).Do(
                ci => ci.Arg<ContainerBuilder>().RegisterType(ci.Arg<Type>()).As<IMockTransientDependency>());

            // act
            discoveryTask.AddConvention(convention);
            discoveryTask.Execute(context);

            // assert
            var container = builder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockTransientDependency))).SingleOrDefault(
                    );

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