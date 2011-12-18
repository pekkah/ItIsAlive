namespace Sakura.Framework.Tests.Bootstrapping.Tasks
{
    using System.Collections.Generic;

    using Autofac;

    using Machine.Fakes;
    using Machine.Specifications;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Tasks;
    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.Conventions;

    public class Given_context_and_task_when_executed : WithSubject<InitializationTaskManager>
    {
        private static IInitializationTask task;

        private static InitializationTaskContext taskContext;

        private Establish context = () =>
            {
                var builder = new ContainerBuilder();
                var conventions = An<IEnumerable<IRegistrationConvention>>();

                taskContext = new InitializationTaskContext(builder, conventions);

                task = An<IInitializationTask>();

                Subject.AddTask(task);
            };

        private Because of = () => Subject.Execute(taskContext);

        private It should_execute_task = () => task.WasToldTo(t => t.Execute(taskContext));
    }
}