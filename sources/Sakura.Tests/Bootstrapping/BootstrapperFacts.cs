namespace Sakura.Framework.Tests.Bootstrapping
{
    using Autofac;

    using NSubstitute;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Tasks;

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