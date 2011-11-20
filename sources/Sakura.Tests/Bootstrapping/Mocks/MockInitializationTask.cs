namespace Sakura.Framework.Tests.Bootstrapping.Mocks
{
    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.DefaultTypes;

    public class MockInitializationTask : IInitializationTask, ITransientDependency
    {
        public bool WasExecuted { get; private set; }

        public void Execute(InitializationTaskContext context)
        {
            this.WasExecuted = true;
        }
    }
}