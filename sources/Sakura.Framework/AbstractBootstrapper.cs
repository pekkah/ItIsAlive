namespace Sakura.Framework
{
    using System.Collections.Generic;

    using Autofac;

    using Sakura.Framework.Registration;
    using Sakura.Framework.Tasks;

    public abstract class AbstractBootstrapper
    {
        private readonly TaskEngine taskEngine;

        private IContainer container;

        protected AbstractBootstrapper()
        {
            this.taskEngine = new TaskEngine();
            this.Policies = new HashSet<IRegistrationPolicy>();
        }

        public ISet<IRegistrationPolicy> Policies { get; private set; }

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

            var context = new InitializationTaskContext(builder, this.Policies);
            this.taskEngine.Execute(context);

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