namespace ItIsAlive.Bootstrapping.Tasks
{
    public interface IInitializationTask
    {
        void Execute(InitializationTaskContext context);
    }
}