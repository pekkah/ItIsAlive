namespace Sakura.Framework.Tasks.Types
{
    using Sakura.Framework.Dependencies.DefaultTypes;

    public interface IStartupTask : ITransientDependency
    {
        void Execute();
    }
}