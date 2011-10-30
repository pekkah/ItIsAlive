namespace Fugu.Framework.Tests.Booting
{
    using Autofac;

    using Fugu.Framework.Tasks;

    using NSubstitute;

    using NUnit.Framework;

    [TestFixture]
    public class When_starting
    {
        private Bootstrapper bootstrapper;

        private IStartupTask startupTask;

        [SetUp]
        public void Setup()
        {
            var testInitializationTask = Substitute.For<IInitializationTask>();

            this.startupTask = Substitute.For<IStartupTask>();

            // setup
            testInitializationTask.Execute(
                Arg.Do<InitializationTaskContext>(
                    context => context.Builder.RegisterInstance(this.startupTask).As<IStartupTask>()));
            
            // initialize and start
            this.bootstrapper = new Bootstrapper();
            this.bootstrapper.AddTask(testInitializationTask);

            this.bootstrapper.Initialize();
            this.bootstrapper.Start();
        }

        [Test]
        public void should_execute_startup_tasks_from_container()
        {
            this.startupTask.Received().Execute();
        }

    }
}