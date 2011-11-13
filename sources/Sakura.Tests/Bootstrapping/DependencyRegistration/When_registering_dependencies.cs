namespace Sakura.Framework.Tests.Bootstrapping.DependencyRegistration
{
    using System.Collections.Generic;
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Dependencies.Discovery;
    using Sakura.Framework.Dependencies.Policies;
    using Sakura.Framework.Tests.Bootstrapping.DependencyRegistration.Mocks;

    public class When_registering_dependencies
    {
        private ContainerBuilder containerBuilder;

        private InitializationTaskContext context;

        private IDependencyLocator locator;

        private RegisterDependencies registerDependencies;

        [SetUp]
        public void Setup()
        {
            this.locator = Substitute.For<IDependencyLocator>();

            this.locator.GetDependencies(Arg.Any<IEnumerable<IRegistrationPolicy>>()).Returns(
                new[] { typeof(MockSingleInstanceDependency), typeof(MockTransientDependency) });

            var policies = new IRegistrationPolicy[] { new TransientPolicy(), new SingleInstancePolicy() };

            this.containerBuilder = new ContainerBuilder();
            this.registerDependencies = new RegisterDependencies(this.locator);

            this.context = new InitializationTaskContext(this.containerBuilder, policies);

            this.registerDependencies.Execute(this.context);
        }

        [Test]
        public void should_discover_dependencies_from_locator()
        {
            this.locator.Received().GetDependencies(Arg.Any<IEnumerable<IRegistrationPolicy>>());
        }

        [Test]
        public void should_register_single_instance_as_single_instance()
        {
            var container = this.containerBuilder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockSingleInstanceDependency))).
                    Single();

            registration.Lifetime.Should().BeOfType<RootScopeLifetime>();
        }

        [Test]
        public void should_register_transient_dependency_as_transient()
        {
            var container = this.containerBuilder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockTransientDependency))).Single(
                    );

            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }
    }
}