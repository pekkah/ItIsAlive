namespace Sakura.Framework
{
    using System.Collections.Generic;

    using Autofac;

    using Sakura.Framework.Tasks;

    public abstract class AbstractBootstrapper
    {
        private IContainer container;

        private TaskEngine taskEngine;

        protected AbstractBootstrapper()
        {
            this.taskEngine = new TaskEngine();
        }

        public TaskEngine Tasks
        {
            get
            {
                return this.taskEngine;
            }
        }

        public IContainer Initialize()
        {
            var builder = new ContainerBuilder();

            var context = new InitializationTaskContext(builder);
            this.taskEngine.Execute(context);

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

        public void Shutdown()
        {
            if (this.container != null)
            {
                this.container.Dispose();
            }
        }
    }
}