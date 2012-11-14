namespace Bootstrapper.Bootstrapping.Tasks
{
    public interface IInitializationTask
    {
        void Execute(InitializationTaskContext context);
    }
}