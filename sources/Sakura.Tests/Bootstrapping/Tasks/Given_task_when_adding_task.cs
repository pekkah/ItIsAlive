namespace Sakura.Framework.Tests.Bootstrapping.Tasks
{
    using Machine.Fakes;
    using Machine.Specifications;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Bootstrapping.Tasks.Types;

    public class Given_task_when_adding_task : WithSubject<InitializationTaskManager>
    {
        private static IInitializationTask singleInstanceTask;

        private Establish context = () => { singleInstanceTask = An<IInitializationTask>(); };

        private Because of = () => Subject.AddTask(singleInstanceTask);

        private It should_add_task = () => Subject.Tasks.ShouldContainOnly(singleInstanceTask);
    }
}