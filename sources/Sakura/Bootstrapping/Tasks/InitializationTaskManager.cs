namespace Sakura.Bootstrapping.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sakura.Bootstrapping.Tasks.Discovery;
    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.ExtensionMethods;
    using Sakura.Framework.Dependencies.DefaultTypes;

    public class InitializationTaskManager
    {
        private readonly InitializationTaskListProvider manualInitializationTaskProvider;

        private readonly List<IInitializationTaskProvider> providers;

        public InitializationTaskManager()
        {
            this.manualInitializationTaskProvider = new InitializationTaskListProvider();
            this.providers = new List<IInitializationTaskProvider>() { this.manualInitializationTaskProvider };
        }

        public IEnumerable<IInitializationTask> Tasks
        {
            get
            {
                var tasks = this.providers.SelectMany(source => source.Tasks);

                return tasks;
            }
        }

        public void AddTask(IInitializationTask task)
        {
            if (!this.TryAddTask(task))
            {
                throw new InvalidOperationException(
                    "Task implementing ISingleInstanceDependency can exists in the tasks list only once.");
            }
        }

        public void AddProvider(IInitializationTaskProvider provider)
        {
            this.providers.Add(provider);
        }

        public void Execute(InitializationTaskContext context)
        {
            foreach (var task in this.Tasks)
            {
                task.Execute(context);
            }
        }

        public bool TryAddTask(IInitializationTask task)
        {
            var taskType = task.GetType();

            // single instance tasks can only exists once in the list of tasks
            if (taskType.HasInterface(typeof(ISingleInstanceDependency)))
            {
                var exists = this.Tasks.Any(t => t.GetType() == taskType);

                if (exists)
                {
                    // task already added
                    return false;
                }
            }

            this.manualInitializationTaskProvider.Add(task);
            return true;
        }

        public void ReplaceTask<TTarget>(IInitializationTask with) where TTarget : IInitializationTask
        {
            var target = this.manualInitializationTaskProvider.GetByType<TTarget>().SingleOrDefault();

            if (target == null)
            {
                throw new InvalidOperationException(
                    string.Format("Task manager does not contain task with type '{0}'", typeof(TTarget).FullName));
            }

            this.manualInitializationTaskProvider.Remove(target);
            this.manualInitializationTaskProvider.Add(with);
        }

        public void Remove<T>()
        {
            var target = this.manualInitializationTaskProvider.GetByType<T>().SingleOrDefault();

            if (target == null)
            {
                throw new InvalidOperationException(
                    string.Format("Task manager does not contain task with type '{0}'", typeof(T).FullName));
            }

            this.manualInitializationTaskProvider.Remove(target);
        }
    }
}