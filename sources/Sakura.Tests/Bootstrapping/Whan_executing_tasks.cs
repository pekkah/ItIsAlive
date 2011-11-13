namespace Sakura.Framework.Tests.Bootstrapping
{
    using System.Collections.Generic;

    using Autofac;

    using NSubstitute;

    using NUnit.Framework;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Policies;

    [TestFixture]
    public class Whan_executing_tasks
    {
        private InitializationTaskManager initializationTaskManager;

        [SetUp]
        public void Setup()
        {
            this.initializationTaskManager = new InitializationTaskManager();
        }

        [Test]
        public void should_execute_tasks()
        {
            var task = Substitute.For<IInitializationTask, ISingleInstanceDependency>();
            var policies = Substitute.For<IEnumerable<IRegistrationPolicy>>();

            var builder = new ContainerBuilder();
            var context = new InitializationTaskContext(builder, policies);

            this.initializationTaskManager.AddTask(task);

            this.initializationTaskManager.Execute(context);

            task.Received().Execute(context);
        }
    }
}