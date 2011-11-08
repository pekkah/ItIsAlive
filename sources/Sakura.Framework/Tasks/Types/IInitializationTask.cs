namespace Sakura.Framework.Tasks.Types
{
    public interface IInitializationTask
    {
        void Execute(InitializationTaskContext context);
    }
}