namespace Sakura.Framework.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.ExtensionMethods;

    public class TaskEngine
    {
        private readonly TaskListSource manualTasks;

        private readonly List<ITaskSource> sources;

        public TaskEngine()
        {
            this.manualTasks = new TaskListSource();
            this.sources = new List<ITaskSource>() { this.manualTasks };
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

        public void AddTaskSource(ITaskSource taskSource)
        {
            this.sources.Add(taskSource);
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

            this.manualTasks.Add(task);
            return true;
        }
    }
}