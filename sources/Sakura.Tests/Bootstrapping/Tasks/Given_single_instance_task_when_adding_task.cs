namespace Sakura.Framework.Tests.Bootstrapping.Tasks
{
    using System;

    using Machine.Fakes;
    using Machine.Specifications;

    using NSubstitute;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Bootstrapping.Tasks.Types;

    public class Given_single_instance_task_when_adding_task : WithSubject<InitializationTaskManager>
    {
        private static Exception exception;

        private static IInitializationTask singleInstanceTask;

        private Establish context = () => { singleInstanceTask = Substitute.For<ISingleInstanceInitializationTask>(); };

        private Because of = () =>
            {
                Subject.AddTask(singleInstanceTask);
                exception = Catch.Exception(() => Subject.AddTask(singleInstanceTask));
            };

        private It should_add_once = () => Subject.Tasks.ShouldContainOnly(singleInstanceTask);

        private It should_fail_adding_twice = () => exception.ShouldBeOfType<InvalidOperationException>();
    }
}