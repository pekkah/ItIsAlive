namespace Fugu.Framework.Tasks
{
    using Fugu.Framework.Dependencies;

    public interface IInitializationTask : ITransientDependency
    {
        void Execute(InitializationTaskContext context);
    }

    public interface IStartupTask : ITransientDependency
    {
        void Execute();
    }
}