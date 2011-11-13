namespace Sakura.Bootstrapping.Tasks.Discovery
{
    using System.Collections.Generic;

    using Sakura.Bootstrapping.Tasks.Types;

    public interface IInitializationTaskSource
    {
        IEnumerable<IInitializationTask> GetTasks();
    }
}