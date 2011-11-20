namespace Sakura.Bootstrapping.Tasks.Types
{
    using Sakura.Framework.Dependencies.DefaultTypes;

    public interface IInitializationTask
    {
        void Execute(InitializationTaskContext context);
    }

    public interface ISingleInstanceInitializationTask : IInitializationTask, ISingleInstanceDependency
    {
        
    }
}