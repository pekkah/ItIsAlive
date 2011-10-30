namespace Sakura.Framework.Tasks
{
    using Sakura.Framework.Dependencies;

    public interface IInitializationTask : ITransientDependency
    {
        void Execute(InitializationTaskContext context);
    }

    public interface IStartupTask : ITransientDependency
    {
        void Execute();
    }
}