namespace ItIsAlive.Tasks
{
    public interface IInitializationTask
    {
        void Execute(InitializationTaskContext context);
    }
}