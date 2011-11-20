namespace Sakura.Bootstrapping.Setup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Autofac;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Tasks;
    using Sakura.Bootstrapping.Tasks.Discovery;
    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Dependencies.Conventions;
    using Sakura.Framework.Dependencies.Discovery;

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
            this.bootstrapper.TaskManager.AddTask(new RegisterDependencies(assemblyLocator));

            var types = this.dependencySetup.GetTypes();
            var typeLocator = new ListLocator(types.ToArray());
            this.bootstrapper.TaskManager.AddTask(new RegisterDependencies(typeLocator));

            // load initialization tasks from locator
            this.bootstrapper.TaskManager.AddProvider(new DependencyLocatorProvider(assemblyLocator, this.bootstrapper.Conventions));
            this.bootstrapper.TaskManager.AddProvider(new DependencyLocatorProvider(typeLocator, this.bootstrapper.Conventions));

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

        public ISetupBootstrapper Tasks(Action<InitializationTaskManager> manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            manager(this.bootstrapper.TaskManager);

            return this;
        }

        public ISetupBootstrapper Conventions(Action<ISet<IRegistrationConvention>> conventions)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException("conventions");
            }

            conventions(this.bootstrapper.Conventions);

            return this;
        }
    }
}