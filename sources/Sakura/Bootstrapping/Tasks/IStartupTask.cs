namespace Sakura.Bootstrapping.Tasks
{
    using Sakura.Composition.Markers;

    public interface IStartupTask : ITransientDependency
    {
        void Execute();
    }
}