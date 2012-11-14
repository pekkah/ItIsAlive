namespace Bootstrapper.Framework.Tests.Bootstrapping
{
    using Autofac;

    using Bootstrapper.Bootstrapping;
    using Bootstrapper.Bootstrapping.Tasks;

    using NSubstitute;

    using Xunit;

    public class BootstrapperFacts
    {
        [Fact]
        public void should_execute_discovered_startup_task()
        {
            var bootstrapper = new Bootstrapper();
            var startupTask = Substitute.For<IStartupTask>();

            var register = new ActionTask(
                context => context.Builder.RegisterInstance(startupTask)
                                  .AsImplementedInterfaces());

            bootstrapper.Tasks.Add(register);
            bootstrapper.Initialize();
            bootstrapper.Start();

            startupTask.Received().Execute();
        }

        [Fact]
        public void should_execute_initialization_task()
        {
            var bootstrapper = new Bootstrapper();
            var task = Substitute.For<IInitializationTask>();

            bootstrapper.Tasks.Add(task);
            bootstrapper.Initialize();

            task.Received().Execute(Arg.Any<InitializationTaskContext>());
        }
    }
}