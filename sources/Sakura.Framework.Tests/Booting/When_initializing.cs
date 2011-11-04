namespace Fugu.Framework.Tests.Booting
{
    using Autofac;

    using NSubstitute;

    using NUnit.Framework;

    using Sakura.Framework;
    using Sakura.Framework.Tasks;

    [TestFixture]
    public class When_initializing
    {
        private Bootstrapper bootstrapper;

        private IContainer container;

        private IInitializationTask initializationTask1;

        private IInitializationTask initializationTaskDependency;

        private ITaskSource taskSource;

        private IInitializationTask taskInTaskSource;

        [SetUp]
        public void Setup()
        {
            this.initializationTask1 = Substitute.For<IInitializationTask>();
            this.taskInTaskSource = Substitute.For<IInitializationTask>();
            this.taskSource = Substitute.For<ITaskSource>();
            this.initializationTaskDependency = Substitute.For<IInitializationTask>();

            // initialization task source
            this.taskSource.GetTasks().Returns(new[] { this.taskInTaskSource });

            // initialize bootstrapper
            this.bootstrapper = new Bootstrapper();

            // manually added initialization tasks
            this.bootstrapper.Tasks.AddTask(this.initializationTask1);

            // initialization tasks discovery
            this.bootstrapper.Tasks.AddTaskSource(this.taskSource);

            // executes initialization tasks
            this.container = this.bootstrapper.Initialize();
        }

        [Test]
        public void should_execute_initialization_tasks()
        {
            this.initializationTask1.Received().Execute(Arg.Any<InitializationTaskContext>());
        }

        [Test]
        public void should_execute_tasks_from_custom_sources()
        {
            this.taskInTaskSource.Received().Execute(Arg.Any<InitializationTaskContext>());
        }

        [Test]
        public void should_execute_tasks_with_initialization_task_context()
        {
            this.initializationTask1.Received().Execute(Arg.Any<InitializationTaskContext>());
        }

        [Test]
        public void should_get_tasks_from_custom_sources()
        {
            this.taskSource.Received().GetTasks();
        }
    }
}