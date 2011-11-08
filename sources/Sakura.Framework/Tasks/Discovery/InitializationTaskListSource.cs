namespace Sakura.Framework.Tasks.Discovery
{
    using System.Collections.Generic;

    using Sakura.Framework.Tasks.Types;

    public class InitializationTaskListSource : IInitializationTaskSource
    {
        private readonly List<IInitializationTask> taskList;

        public InitializationTaskListSource()
        {
            this.taskList = new List<IInitializationTask>();
        }

        public void Add(IInitializationTask task)
        {
            this.taskList.Add(task);
        }

        public IEnumerable<IInitializationTask> GetTasks()
        {
            return this.taskList;
        }
    }
}