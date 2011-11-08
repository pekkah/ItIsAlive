namespace Sakura.Framework.Tasks.Discovery
{
    using System.Collections.Generic;

    using Sakura.Framework.Tasks.Types;

    public interface IInitializationTaskSource
    {
        IEnumerable<IInitializationTask> GetTasks();
    }
}