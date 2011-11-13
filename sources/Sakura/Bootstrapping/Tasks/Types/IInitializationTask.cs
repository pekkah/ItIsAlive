namespace Sakura.Bootstrapping.Tasks.Types
{
    public interface IInitializationTask
    {
        void Execute(InitializationTaskContext context);
    }
}