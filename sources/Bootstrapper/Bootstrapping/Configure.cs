namespace Bootstrapper.Bootstrapping
{
    using System;
    using System.Collections.Generic;

    using Autofac;

    using Composition;
    using Composition.Discovery;

    using Tasks;

    public class Configure : IConfigureBootstrapper
    {
        private readonly Bootstrapper bootstrapper;

        private readonly ConfigureDependencies configureDependencies;

        private readonly List<IRegistrationConvention> conventions;

        private Action<IContainer> exposeContainer;

        public Configure()
        {
            bootstrapper = new Bootstrapper();
            configureDependencies = new ConfigureDependencies();
            conventions = new List<IRegistrationConvention>();
        }

        public IConfigureBootstrapper Dependencies(Action<IConfigureDependencies> from)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            from(configureDependencies);

            return this;
        }

        public IConfigureBootstrapper ExposeContainer(Action<IContainer> exposedContainer)
        {
            if (exposedContainer == null)
            {
                throw new ArgumentNullException("exposedContainer");
            }

            exposeContainer = exposedContainer;

            return this;
        }

        public Bootstrapper Start()
        {
            var assemblies = configureDependencies.SourceAssemblies;
            var types = configureDependencies.SourceTypes;

            var assemblyLocator = new AssemblyLocator(assemblies);
            var discoverFromAssemblies = new DefaultDependencyDiscoveryTask(assemblyLocator);

            var typeLocator = new ListLocator(types);
            var discoverFromTypes = new DefaultDependencyDiscoveryTask(typeLocator);

            // add custom conventions to discovery tasks
            foreach (var convention in conventions)
            {
                discoverFromAssemblies.AddConvention(convention);
                discoverFromTypes.AddConvention(convention);
            }

            bootstrapper.Tasks.Add(discoverFromAssemblies);
            bootstrapper.Tasks.Add(discoverFromTypes);

            // execute initialization tasks
            var container = bootstrapper.Initialize();

            // expose container
            if (exposeContainer != null)
            {
                exposeContainer(container);
            }

            // start
            bootstrapper.Start();

            return bootstrapper;
        }

        public IConfigureBootstrapper Conventions(Action<ICollection<IRegistrationConvention>> configureConventions)
        {
            if (configureConventions == null)
            {
                throw new ArgumentNullException("configureConventions");
            }

            configureConventions(conventions);

            return this;
        }

        public IConfigureBootstrapper Tasks(Action<ICollection<IInitializationTask>> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException("tasks");
            }

            tasks(bootstrapper.Tasks);

            return this;
        }
    }
}