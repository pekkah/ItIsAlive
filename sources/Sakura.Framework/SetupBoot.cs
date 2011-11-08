namespace Sakura.Framework
{
    using System;
    using System.Linq;

    using Autofac;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Registration;
    using Sakura.Framework.Tasks;
    using Sakura.Framework.Tasks.Initialization;

    public class SetupBoot : ISetupBootstrapper
    {
        private Action<IContainer> exposeContainer;

        private Bootstrapper bootstrapper;

        private SetupDependencies dependencySetup;

        public SetupBoot()
        {
            this.bootstrapper = new Bootstrapper();
            this.dependencySetup = new SetupDependencies();
        }

        public ISetupBootstrapper TryTask(IInitializationTask task)
        {
            this.bootstrapper.Tasks.TryAddTask(task);

            return this;
        }

        public ISetupBootstrapper ExposeContainer(Action<IContainer> exposeTo)
        {
            if (exposeTo == null)
            {
                throw new ArgumentNullException("exposeTo");
            }

            this.exposeContainer = exposeTo;

            return this;
        }

        public ISetupBootstrapper AddPolicy(IRegistrationPolicy policy)
        {
            if (policy == null)
            {
                throw new ArgumentNullException("policy");
            }

            this.bootstrapper.Policies.Add(policy);

            return this;
        }

        public ISetupBootstrapper Dependencies(Action<ISetupDependencies> setupDependencies)
        {
            if (setupDependencies == null)
            {
                throw new ArgumentNullException("setupDependencies");
            }

            setupDependencies(this.dependencySetup);

            return this;
        }

        public Bootstrapper Start()
        {
            var assemblies = this.dependencySetup.GetAssemblies();
            var assemblyLocator = new AssemblyLocator(assemblies);
            this.bootstrapper.Tasks.AddTask(new RegisterDependenciesTask(assemblyLocator));

            var types = this.dependencySetup.GetTypes();
            var typeLocator = new TypeLocator(types.ToArray());
            this.bootstrapper.Tasks.AddTask(new RegisterDependenciesTask(typeLocator));
            
            // load initialization tasks from locator
            this.bootstrapper.Tasks.AddTaskSource(new DependencyLocatorSource(assemblyLocator, this.bootstrapper.Policies));
            this.bootstrapper.Tasks.AddTaskSource(new DependencyLocatorSource(typeLocator, this.bootstrapper.Policies));

            // execute initialization tasks
            var container = this.bootstrapper.Initialize();

            // expose container
            if (this.exposeContainer != null)
            {
                this.exposeContainer(container);
            }

            // start
            this.bootstrapper.Start();

            return this.bootstrapper;
        }

        public ISetupBootstrapper Task(IInitializationTask task)
        {
            this.bootstrapper.Tasks.AddTask(task);

            return this;
        }
    }
}