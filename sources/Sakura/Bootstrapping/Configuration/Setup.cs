namespace Sakura.Bootstrapping.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Autofac;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Tasks;
    using Sakura.Bootstrapping.Tasks.Discovery;
    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Dependencies.Discovery;
    using Sakura.Framework.Dependencies.Policies;

    public class Setup : ISetupBootstrapper
    {
        private readonly Bootstrapper bootstrapper;

        private readonly SetupDependencies dependencySetup;

        private Action<IContainer> exposeContainer;

        public Setup()
        {
            this.bootstrapper = new Bootstrapper();
            this.dependencySetup = new SetupDependencies();
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

        public ISetupBootstrapper ExposeContainer(Action<IContainer> exposeTo)
        {
            if (exposeTo == null)
            {
                throw new ArgumentNullException("exposeTo");
            }

            this.exposeContainer = exposeTo;

            return this;
        }

        public Bootstrapper Start()
        {
            var assemblies = this.dependencySetup.GetAssemblies();
            var assemblyLocator = new AssemblyLocator(assemblies);
            this.bootstrapper.InitializationTasks.AddTask(new RegisterDependencies(assemblyLocator));

            var types = this.dependencySetup.GetTypes();
            var typeLocator = new ListLocator(types.ToArray());
            this.bootstrapper.InitializationTasks.AddTask(new RegisterDependencies(typeLocator));

            // load initialization tasks from locator
            this.bootstrapper.InitializationTasks.AddTaskSource(new DependencyLocatorSource(assemblyLocator, this.bootstrapper.RegistrationPolicies));
            this.bootstrapper.InitializationTasks.AddTaskSource(new DependencyLocatorSource(typeLocator, this.bootstrapper.RegistrationPolicies));

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

        public ISetupBootstrapper Tasks(Action<InitializationTaskManager> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException("tasks");
            }

            tasks(this.bootstrapper.InitializationTasks);

            return this;
        }

        public ISetupBootstrapper RegistrationPolicies(Action<ISet<IRegistrationPolicy>> policies)
        {
            if (policies == null)
            {
                throw new ArgumentNullException("policies");
            }

            policies(this.bootstrapper.RegistrationPolicies);

            return this;
        }
    }
}