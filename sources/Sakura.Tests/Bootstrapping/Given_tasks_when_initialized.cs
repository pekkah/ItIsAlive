namespace Sakura.Framework.Tests.Bootstrapping
{
    using System.Linq;

    using Machine.Fakes;
    using Machine.Specifications;

    using NSubstitute;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Tasks.Discovery;
    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.Discovery;
    using Sakura.Framework.Tests.Bootstrapping.Mocks;

    public class Given_tasks_when_initialized : WithSubject<Bootstrapper>
    {
        private static IInitializationTask initializationTask1;

        private static IInitializationTask initializationTaskDependency;

        private static IInitializationTaskProvider initializationTaskProvider;

        private static IInitializationTask taskInTaskSource;

        private Establish context = () =>
            {
                // setup source
                taskInTaskSource = An<IInitializationTask>();
                initializationTaskProvider = An<IInitializationTaskProvider>();
                initializationTaskProvider.WhenToldTo(source => source.Tasks).Return(new[] { taskInTaskSource });

                // setup manual task
                initializationTask1 = An<IInitializationTask>();

                // dependency
                Subject.TaskManager.AddProvider(
                    new DependencyLocatorProvider(new ListLocator(typeof(MockInitializationTask)), Subject.Conventions));

                // manually added initialization tasks
                Subject.TaskManager.AddTask(initializationTask1);

                // initialization tasks discovery
                Subject.TaskManager.AddProvider(initializationTaskProvider);
            };

        private Because of = () => Subject.Initialize();

        private It should_execute_initialization_tasks =
            () => initializationTask1.WasToldTo(t => t.Execute(Arg.Any<InitializationTaskContext>()));

        private It should_execute_task_from_dependencies = () =>
            {
                var dependencyTask = Subject.TaskManager.Tasks.OfType<MockInitializationTask>().Single();

                dependencyTask.WasExecuted.ShouldBeTrue();
            };

        private It should_execute_tasks_from_custom_sources =
            () => taskInTaskSource.WasToldTo(t => t.Execute(Arg.Any<InitializationTaskContext>()));
    }
}