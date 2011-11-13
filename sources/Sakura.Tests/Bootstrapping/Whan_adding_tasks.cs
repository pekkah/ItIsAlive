﻿namespace Sakura.Framework.Tests.Bootstrapping
{
    using System;

    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.DefaultTypes;

    [TestFixture]
    public class Whan_adding_tasks
    {
        private InitializationTaskManager initializationTaskManager;

        [SetUp]
        public void Setup()
        {
            this.initializationTaskManager = new InitializationTaskManager();
        }

        [Test]
        public void should_add_single_instance_task()
        {
            var task = Substitute.For<IInitializationTask, ISingleInstanceDependency>();

            this.initializationTaskManager.AddTask(task);

            this.initializationTaskManager.Tasks.Should().Contain(task);
        }

        [Test]
        public void should_add_task()
        {
            var task = Substitute.For<IInitializationTask, ITransientDependency>();

            this.initializationTaskManager.AddTask(task);

            this.initializationTaskManager.Tasks.Should().Contain(task);
        }

        [Test]
        public void should_fail_adding_single_instance_task_twice()
        {
            var task = Substitute.For<IInitializationTask, ISingleInstanceDependency>();

            this.initializationTaskManager.AddTask(task);
            Assert.Throws<InvalidOperationException>(() => this.initializationTaskManager.AddTask(task));
        }
    }
}