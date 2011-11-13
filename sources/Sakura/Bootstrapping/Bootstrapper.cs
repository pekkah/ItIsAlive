namespace Sakura.Bootstrapping
{
    using System.Collections.Generic;

    using Autofac;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.Policies;

    public class Bootstrapper
    {
        private readonly InitializationTaskManager initializationTaskManager;

        private IContainer container;

        public Bootstrapper()
        {
            this.initializationTaskManager = new InitializationTaskManager();
            this.RegistrationPolicies = new HashSet<IRegistrationPolicy>
                {
                    new AsSelfPolicy(), 
                    new SingleInstancePolicy(), 
                    new TransientPolicy()
                };
        }

        public ISet<IRegistrationPolicy> RegistrationPolicies
        {
            get;
            private set;
        }

        public InitializationTaskManager InitializationTasks
        {
            get
            {
                return this.initializationTaskManager;
            }
        }

        public IContainer Initialize()
        {
            var builder = new ContainerBuilder();

            var context = new InitializationTaskContext(builder, this.RegistrationPolicies);
            this.initializationTaskManager.Execute(context);

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