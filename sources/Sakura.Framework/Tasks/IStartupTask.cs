namespace Sakura.Framework.Tasks
{
    using Sakura.Framework.Dependencies;

    public interface IStartupTask : ITransientDependency
    {
        void Execute();
    }
}