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
        private readonly InitializationTaskListSource manualInitializationTasks;

        private readonly List<IInitializationTaskSource> sources;

        public InitializationTaskManager()
        {
            this.manualInitializationTasks = new InitializationTaskListSource();
            this.sources = new List<IInitializationTaskSource>() { this.manualInitializationTasks };
        }

        public IEnumerable<IInitializationTask> Tasks
        {
            get
            {
                var tasks = this.sources.SelectMany(source => source.GetTasks());

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

        public void AddTaskSource(IInitializationTaskSource initializationTaskSource)
        {
            this.sources.Add(initializationTaskSource);
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

            this.manualInitializationTasks.Add(task);
            return true;
        }

        public void ReplaceTask<TTarget>(IInitializationTask with) where TTarget : IInitializationTask
        {
            var target = this.manualInitializationTasks.GetByType<TTarget>().SingleOrDefault();

            if (target == null)
            {
                throw new InvalidOperationException(
                    string.Format("Task manager does not contain task with type '{0}'", typeof(TTarget).FullName));
            }

            this.manualInitializationTasks.Remove(target);
            this.manualInitializationTasks.Add(with);
        }
    }
}