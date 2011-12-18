namespace Sakura.Framework.Tests.Bootstrapping.DependencyDiscovery
{
    using System.Collections.Generic;
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using Machine.Fakes;
    using Machine.Specifications;

    using NSubstitute;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Dependencies.Conventions;
    using Sakura.Framework.Dependencies.Discovery;
    using Sakura.Framework.Tests.Bootstrapping.BehaviorConfigs;
    using Sakura.Framework.Tests.Bootstrapping.Mocks;

    public class Given_dependencies_when_registering_dependencies : WithSubject<RegisterDependencies>
    {
        private static ContainerBuilder containerBuilder;

        private static InitializationTaskContext taskContext;

        private static IContainer container;

        private Establish context = () =>
            {
                With<DefaultConventions>();
                With<DependencyLocation>();

                containerBuilder = new ContainerBuilder();

                taskContext = new InitializationTaskContext(
                    containerBuilder, The<IEnumerable<IRegistrationConvention>>());
            };

        private Because of = () =>
            {
                Subject.Execute(taskContext);
                container = containerBuilder.Build();
            };

        private It should_discover_dependencies_from_locator =
            () => The<IDependencyLocator>().Received().GetDependencies(The<IEnumerable<IRegistrationConvention>>());

        private It should_register_single_instance_as_single_instance = () =>
            {
                var registration =
                    container.ComponentRegistry.RegistrationsFor(
                        new TypedService(typeof(IMockSingleInstanceDependency))).Single();

                registration.Lifetime.ShouldBeOfType<RootScopeLifetime>();
            };

        private It should_register_transient_dependency_as_transient = () =>
            {
                var registration =
                    container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockTransientDependency))).
                        Single();

                registration.Lifetime.ShouldBeOfType<CurrentScopeLifetime>();
            };
    }
}