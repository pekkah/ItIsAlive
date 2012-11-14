namespace ItIsAlive.Framework.Tests.Bootstrapping
{
    using Autofac;
    using NSubstitute;
    using Tasks;
    using Xunit;

    public class BootstrapperFacts
    {
        [Fact]
        public void should_execute_discovered_startup_task()
        {
            var bootstrapper = new TheThing();
            var startupTask = Substitute.For<IStartupTask>();

            var register = new ActionTask(
                context => context.Builder.RegisterInstance(startupTask)
                                  .AsImplementedInterfaces());

            bootstrapper.Sequence.Add(register);
            bootstrapper.Initialize();
            bootstrapper.Start();

            startupTask.Received().Execute();
        }

        [Fact]
        public void should_execute_initialization_task()
        {
            var bootstrapper = new TheThing();
            var task = Substitute.For<IInitializationTask>();

            bootstrapper.Sequence.Add(task);
            bootstrapper.Initialize();

            task.Received().Execute(Arg.Any<InitializationTaskContext>());
        }
    }
}