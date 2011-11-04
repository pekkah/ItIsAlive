namespace Sakura.Framework.Tests.Booting
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Tasks;

    [TestFixture]
    public class Whan_adding_tasks
    {
        private TaskEngine taskEngine;

        [SetUp]
        public void Setup()
        {
            this.taskEngine = new TaskEngine();
        }

        [Test]
        public void should_add_task()
        {
            var task = Substitute.For<IInitializationTask, ITransientDependency>();

            this.taskEngine.AddTask(task);

            this.taskEngine.Tasks.Should().Contain(task);
        }

        [Test]
        public void should_add_single_instance_task()
        {
            var task = Substitute.For<IInitializationTask, ISingleInstanceDependency>();

            this.taskEngine.AddTask(task);

            this.taskEngine.Tasks.Should().Contain(task);
        }

        [Test]
        public void should_fail_adding_single_instance_task_twice()
        {
            var task = Substitute.For<IInitializationTask, ISingleInstanceDependency>();

            this.taskEngine.AddTask(task);
            Assert.Throws<InvalidOperationException>(() => this.taskEngine.AddTask(task));
        }
    }
}