namespace Sakura.Framework.Tasks.Discovery
{
    using System.Collections.Generic;
    using System.Linq;

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

        public void Remove(IInitializationTask task)
        {
            if (this.taskList.Contains(task))
            {
                this.taskList.Remove(task);
            }
        }

        public IEnumerable<IInitializationTask> GetTasks()
        {
            return this.taskList;
        }

        public IEnumerable<IInitializationTask> GetByType<T>()
        {
            return this.taskList.Where(task => typeof(T).IsAssignableFrom(task.GetType()));
        }
    }
}