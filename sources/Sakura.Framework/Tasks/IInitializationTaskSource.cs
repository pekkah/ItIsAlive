namespace Sakura.Framework.Tasks
{
    using System.Collections.Generic;

    public interface IInitializationTaskSource
    {
        IEnumerable<IInitializationTask> GetTasks();
    }
}