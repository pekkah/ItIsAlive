namespace Sakura.Bootstrapping.Tasks.Discovery
{
    using System.Collections.Generic;
    using System.Linq;

    using Sakura.Bootstrapping.Tasks.Types;

    public class InitializationTaskListProvider : IInitializationTaskProvider
    {
        private readonly List<IInitializationTask> taskList;

        public InitializationTaskListProvider()
        {
            this.taskList = new List<IInitializationTask>();
        }

        public IEnumerable<IInitializationTask> Tasks
        {
            get
            {
                return this.taskList;
            }
        }

        public void Add(IInitializationTask task)
        {
            this.taskList.Add(task);
        }

        public IEnumerable<IInitializationTask> GetByType<T>()
        {
            return this.taskList.Where(task => typeof(T).IsAssignableFrom(task.GetType()));
        }

        public void Remove(IInitializationTask task)
        {
            if (this.taskList.Contains(task))
            {
                this.taskList.Remove(task);
            }
        }
    }
}