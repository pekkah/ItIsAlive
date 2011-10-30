namespace Sakura.Framework
{
    using System.Collections.Generic;
    using System.Linq;

    using Autofac;

    using Sakura.Framework.Tasks;

    public abstract class AbstractBootstrapper
    {
        private readonly List<IInitializationTaskSource> initializationTaskSources;

        private readonly InitializationTaskSource initializationTasks;

        private IContainer container;

        protected AbstractBootstrapper()
        {
            this.initializationTasks = new InitializationTaskSource();
            this.initializationTaskSources = new List<IInitializationTaskSource> { this.initializationTasks };
        }

        public void AddTask(IInitializationTask task)
        {
            this.initializationTasks.AddTask(task);
        }

        public void AddTaskSource(IInitializationTaskSource taskSource)
        {
            this.initializationTaskSources.Add(taskSource);
        }

        public IContainer Initialize()
        {
            var builder = new ContainerBuilder();

            var tasks = this.initializationTaskSources.SelectMany(source => source.GetTasks());

            var context = new InitializationTaskContext(builder);

            foreach (var task in tasks)
            {
                task.Execute(context);
            }

            return this.container = builder.Build();
        }

        public void Start()
        {
            // startup tasks are resolved from container
            var tasks = this.container.Resolve<IEnumerable<IStartupTask>>();

            foreach (var task in tasks)
            {
                task.Execute();
            }
        }
    }
}