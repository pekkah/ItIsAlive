namespace Sakura.Bootstrapping
{
    using System;
    using System.Collections.Generic;

    using Autofac;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Composition;
    using Sakura.Composition.Discovery;

    public class Configure : IConfigureBootstrapper
    {
        private readonly Bootstrapper bootstrapper;

        private Action<IContainer> exposeContainer;

        private readonly ConfigureDependencies configureDependencies;

        private readonly List<IRegistrationConvention> conventions;

        public Configure()
        {
            this.bootstrapper = new Bootstrapper();
            this.configureDependencies = new ConfigureDependencies();
            this.conventions = new List<IRegistrationConvention>();
        }

        public IConfigureBootstrapper Dependencies(Action<IConfigureDependencies> dependencies)
        {
            if (dependencies == null)
            {
                throw new ArgumentNullException("dependencies");
            }

            dependencies(this.configureDependencies);

            return this;
        }

        public IConfigureBootstrapper Conventions(Action<IList<IRegistrationConvention>> configureConventions)
        {
            if (configureConventions == null)
            {
                throw new ArgumentNullException("configureConventions");
            }

            configureConventions(this.conventions);

            return this;
        }

        public IConfigureBootstrapper ExposeContainer(Action<IContainer> exposeTo)
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
            var assemblies = this.configureDependencies.GetAssemblies();
            var types = this.configureDependencies.GetTypes();

            var assemblyLocator = new AssemblyLocator(assemblies);
            var discoverFromAssemblies = new DefaultDependencyDiscoveryTask(assemblyLocator);

            var typeLocator = new ListLocator(types);
            var discoverFromTypes = new DefaultDependencyDiscoveryTask(typeLocator);

            // add custom conventions to discovery tasks
            foreach (var convention in this.conventions)
            {
                discoverFromAssemblies.AddConvention(convention);
                discoverFromTypes.AddConvention(convention);
            }

            this.bootstrapper.Tasks.Add(discoverFromAssemblies);
            this.bootstrapper.Tasks.Add(discoverFromTypes);

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

        public IConfigureBootstrapper Tasks(Action<IList<IInitializationTask>> modifyTasks)
        {
            if (modifyTasks == null)
            {
                throw new ArgumentNullException("modifyTasks");
            }

            modifyTasks(this.bootstrapper.Tasks);

            return this;
        }
    }
}