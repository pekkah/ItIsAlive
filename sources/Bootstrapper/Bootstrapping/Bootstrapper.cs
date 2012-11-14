namespace Bootstrapper.Bootstrapping
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Autofac;

    using Tasks;

    public class Bootstrapper
    {
        private IContainer container;

        public Bootstrapper()
        {
            Tasks = new Collection<IInitializationTask>();
        }

        public Collection<IInitializationTask> Tasks { get; private set; }

        public IContainer Initialize()
        {
            var builder = new ContainerBuilder();

            var context = new InitializationTaskContext(builder);

            foreach (var task in Tasks)
            {
                task.Execute(context);
            }

            return container = builder.Build();
        }

        public void Shutdown()
        {
            if (container != null)
            {
                container.Dispose();
            }
        }

        public void Start()
        {
            // startup tasks are resolved from container
            var tasks = container.Resolve<IEnumerable<IStartupTask>>();

            foreach (var task in tasks)
            {
                task.Execute();
            }
        }
    }
}