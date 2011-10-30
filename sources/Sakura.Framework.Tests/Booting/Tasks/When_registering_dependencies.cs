namespace Fugu.Framework.Tests.Booting.Tasks
{
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using Fugu.Framework.Tests.Booting.Tasks.Mocks;

    using NSubstitute;

    using NUnit.Framework;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Tasks;
    using Sakura.Framework.Tasks.Initialization;

    public class When_registering_dependencies
    {
        private ContainerBuilder containerBuilder;

        private InitializationTaskContext context;

        private IDependencyLocator locator;

        private RegisterDependenciesTask registerDependenciesTask;

        [SetUp]
        public void Setup()
        {
            this.locator = Substitute.For<IDependencyLocator>();

            this.locator.GetDependencies().Returns(
                new[] { typeof(MockSingleInstanceDependency), typeof(MockTransientDependency) });

            this.containerBuilder = new ContainerBuilder();
            this.registerDependenciesTask = new RegisterDependenciesTask(this.locator);

            this.context = new InitializationTaskContext(this.containerBuilder);
        }

        [Test]
        public void should_discover_dependencies_from_locator()
        {
            this.registerDependenciesTask.Execute(this.context);

            this.locator.Received().GetDependencies();
        }

        [Test]
        public void should_register_single_instance_as_single_instance()
        {
            this.registerDependenciesTask.Execute(this.context);

            var container = this.containerBuilder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockSingleInstanceDependency))).
                    Single();

            registration.Lifetime.Should().BeOfType<RootScopeLifetime>();
        }

        [Test]
        public void should_register_transient_dependency_as_transient()
        {
            this.registerDependenciesTask.Execute(this.context);

            var container = this.containerBuilder.Build();
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockTransientDependency))).Single(
                    );

            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }
    }
}