﻿namespace Fugu.Framework.Tests.Booting
{
    using Autofac;

    using Fugu.Framework.Tasks;

    using NSubstitute;

    using NUnit.Framework;

    [TestFixture]
    public class When_initializing
    {
        private Bootstrapper bootstrapper;

        private IInitializationTask initializationTask1;

        private IInitializationTaskSource initializationTaskSource;

        private IInitializationTask taskInTaskSource;

        private IContainer container;

        private IInitializationTask initializationTaskDependency;

        [SetUp]
        public void Setup()
        {
            this.initializationTask1 = Substitute.For<IInitializationTask>();
            this.taskInTaskSource = Substitute.For<IInitializationTask>();
            this.initializationTaskSource = Substitute.For<IInitializationTaskSource>();
            this.initializationTaskDependency = Substitute.For<IInitializationTask>();

            // initialization task source
            this.initializationTaskSource.GetTasks().Returns(new[] { this.taskInTaskSource });

            // initialize bootstrapper
            this.bootstrapper = new Bootstrapper();

            // manually added initialization tasks
            this.bootstrapper.AddTask(this.initializationTask1);

            // initialization tasks discovery
            this.bootstrapper.AddTaskSource(this.initializationTaskSource);

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
        public void should_get_tasks_from_custom_sources()
        {
            this.initializationTaskSource.Received().GetTasks();
        }

        [Test]
        public void should_execute_tasks_with_initialization_task_context()
        {
            this.initializationTask1.Received().Execute(Arg.Any<InitializationTaskContext>());
        }
    }
}