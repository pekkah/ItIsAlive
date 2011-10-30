namespace Fugu.Framework.Tests.Booting.Tasks
{
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using Fugu.Framework.Dependencies;
    using Fugu.Framework.Tasks;
    using Fugu.Framework.Tasks.Initialization;
    using Fugu.Framework.Tests.Booting.Tasks.Mocks;

    using NSubstitute;

    using NUnit.Framework;

    public class When_registering_dependencies
    {
        private ContainerBuilder containerBuilder;

        private IDependencyLocator locator;

        private RegisterDependenciesTask registerDependenciesTask;

        private InitializationTaskContext context;

        [SetUp]
        public void Setup()
        {
            this.locator = Substitute.For<IDependencyLocator>();

            this.locator.GetDependencies().Returns(
                new[] { 
                    typeof(MockSingleInstanceDependency), 
                    typeof(MockTransientDependency)});

            this.containerBuilder = new ContainerBuilder();
            this.registerDependenciesTask = new RegisterDependenciesTask(this.locator);

            this.context = new InitializationTaskContext(this.containerBuilder);
        }

        [Test]
        public void should_discover_dependencies_from_locator()
        {
            this.registerDependenciesTask.Execute(context);

            this.locator.Received().GetDependencies();
        }

        [Test]
        public void should_register_single_instance_as_single_instance()
        {
            this.registerDependenciesTask.Execute(context);

            var container = this.containerBuilder.Build();
            var registration = container
                .ComponentRegistry
                .RegistrationsFor(new TypedService(typeof(IMockSingleInstanceDependency)))
                .Single();

            registration.Lifetime.Should().BeOfType<RootScopeLifetime>();
        }

        [Test]
        public void should_register_transient_dependency_as_transient()
        {
            this.registerDependenciesTask.Execute(context);

            var container = this.containerBuilder.Build();
            var registration = container
                .ComponentRegistry
                .RegistrationsFor(new TypedService(typeof(IMockTransientDependency)))
                .Single();

            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }
    }
}