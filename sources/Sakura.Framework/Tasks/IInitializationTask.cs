namespace Sakura.Framework.Tasks
{
    public interface IInitializationTask
    {
        void Execute(InitializationTaskContext context);
    }
}