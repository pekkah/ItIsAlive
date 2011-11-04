namespace Sakura.Framework.Tasks
{
    using System.Collections.Generic;

    public interface ITaskSource
    {
        IEnumerable<IInitializationTask> GetTasks();
    }
}