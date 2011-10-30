namespace Sakura.Framework.Tasks
{
    using System.Collections.Generic;

    public class InitializationTaskSource : IInitializationTaskSource
    {
        private readonly List<IInitializationTask> taskList;

        public InitializationTaskSource()
        {
            this.taskList = new List<IInitializationTask>();
        }

        public void AddTask(IInitializationTask task)
        {
            this.taskList.Add(task);
        }

        public IEnumerable<IInitializationTask> GetTasks()
        {
            return this.taskList;
        }
    }
}