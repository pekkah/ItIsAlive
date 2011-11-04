namespace Sakura.Framework.Tests.Booting
{
    using Autofac;

    using NSubstitute;

    using NUnit.Framework;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Tasks;

    [TestFixture]
    public class Whan_executing_tasks
    {
        private TaskEngine taskEngine;

        [SetUp]
        public void Setup()
        {
            this.taskEngine = new TaskEngine();
        }

        [Test]
        public void should_execute_tasks()
        {
            var task = Substitute.For<IInitializationTask, ISingleInstanceDependency>();
            var builder = new ContainerBuilder();
            var context = new InitializationTaskContext(builder);

            this.taskEngine.AddTask(task);


            this.taskEngine.Execute(context);

            task.Received().Execute(context);
        }
    }
}