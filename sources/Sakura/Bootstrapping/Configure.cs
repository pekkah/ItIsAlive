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

        private readonly ConfigureDependencies configureDependencies;

        private readonly List<IRegistrationConvention> conventions;

        private Action<IContainer> exposeContainer;

        public Configure()
        {
            this.bootstrapper = new Bootstrapper();
            this.configureDependencies = new ConfigureDependencies();
            this.conventions = new List<IRegistrationConvention>();
        }

        public IConfigureBootstrapper Conventions(Action<ICollection<IRegistrationConvention>> configureConventions)
        {
            if (configureConventions == null)
            {
                throw new ArgumentNullException("configureConventions");
            }

            configureConventions(this.conventions);

            return this;
        }

        public IConfigureBootstrapper Dependencies(Action<IConfigureDependencies> from)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            from(this.configureDependencies);

            return this;
        }

        public IConfigureBootstrapper ExposeContainer(Action<IContainer> exposedContainer)
        {
            if (exposedContainer == null)
            {
                throw new ArgumentNullException("exposedContainer");
            }

            this.exposeContainer = exposedContainer;

            return this;
        }

        public Bootstrapper Start()
        {
            var assemblies = this.configureDependencies.SourceAssemblies;
            var types = this.configureDependencies.SourceTypes;

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

        public IConfigureBootstrapper Tasks(Action<ICollection<IInitializationTask>> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException("tasks");
            }

            tasks(this.bootstrapper.Tasks);

            return this;
        }
    }
}