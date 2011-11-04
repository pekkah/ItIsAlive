namespace Sakura.Framework.Tasks
{
    using System.Collections.Generic;

    public class TaskListSource : ITaskSource
    {
        private readonly List<IInitializationTask> taskList;

        public TaskListSource()
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