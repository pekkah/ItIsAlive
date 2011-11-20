namespace Sakura.Framework.Tests.Bootstrapping
{
    using Autofac;

    using Machine.Fakes;
    using Machine.Specifications;

    using NSubstitute;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Tasks.Types;

    public class Given_startup_task_when_started : WithSubject<Bootstrapper>
    {
        private static IStartupTask startupTask;

        private Establish context = () =>
            {
                var testInitializationTask = An<IInitializationTask>();

                startupTask = An<IStartupTask>();

                // setup
                testInitializationTask.Execute(
                    Arg.Do<InitializationTaskContext>(
                        context => context.Builder.RegisterInstance(startupTask).As<IStartupTask>()));

                // initialize and start
                Subject.TaskManager.AddTask(testInitializationTask);

                Subject.Initialize();
            };

        private Because of = () => Subject.Start();

        private It should_execute_startup_tasks_from_container = () => startupTask.WasToldTo(t => t.Execute());
    }
}