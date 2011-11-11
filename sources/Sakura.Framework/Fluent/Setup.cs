namespace Sakura.Framework.Fluent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Autofac;

    using Sakura.Framework.Dependencies.Discovery;
    using Sakura.Framework.Dependencies.Policies;
    using Sakura.Framework.Tasks;
    using Sakura.Framework.Tasks.Discovery;
    using Sakura.Framework.Tasks.Types;

    public class Setup : ISetupBootstrapper
    {
        private Bootstrapper bootstrapper;

        private SetupDependencies dependencySetup;

        private Action<IContainer> exposeContainer;

        public Setup()
        {
            this.bootstrapper = new Bootstrapper();
            this.dependencySetup = new SetupDependencies();
        }

        public ISetupBootstrapper AddPolicy(IRegistrationPolicy policy)
        {
            if (policy == null)
            {
                throw new ArgumentNullException("policy");
            }

            this.bootstrapper.RegistrationPolicies.Add(policy);

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

        public ISetupBootstrapper Task(IInitializationTask task)
        {
            this.bootstrapper.InitializationTasks.AddTask(task);

            return this;
        }

        public ISetupBootstrapper TryTask(IInitializationTask task)
        {
            this.bootstrapper.InitializationTasks.TryAddTask(task);

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