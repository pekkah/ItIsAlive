namespace ItIsAlive.Bootstrapping.Tasks
{
    using Composition.Markers;

    public interface IStartupTask : ITransientDependency
    {
        void Execute();
    }
}