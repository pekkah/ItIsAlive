namespace Sakura.Bootstrapping
{
    using System.Collections.Generic;

    using Autofac;

    using Sakura.Bootstrapping.Tasks;

    public class Bootstrapper
    {
        private IContainer container;

        public Bootstrapper()
        {
            this.Tasks = new List<IInitializationTask>();
        }

        public List<IInitializationTask> Tasks { get; private set; }

        public IContainer Initialize()
        {
            var builder = new ContainerBuilder();

            var context = new InitializationTaskContext(builder);

            foreach (var task in this.Tasks)
            {
                task.Execute(context);
            }

            return this.container = builder.Build();
        }

        public void Shutdown()
        {
            if (this.container != null)
            {
                this.container.Dispose();
            }
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