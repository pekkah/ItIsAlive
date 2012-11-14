namespace ItIsAlive.Bootstrapping
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using Composition;
    using Composition.Discovery;
    using Tasks;

    public class ItIs : IConfigureBootstrapper
    {
        private readonly Bootstrapper _bootstrapper;

        private readonly ConfigureDependencies _configureDependencies;

        private readonly List<IRegistrationConvention> _conventions;

        private Action<IContainer> _exposeContainer;

        public ItIs()
        {
            _bootstrapper = new Bootstrapper();
            _configureDependencies = new ConfigureDependencies();
            _conventions = new List<IRegistrationConvention>();
        }

        public IConfigureBootstrapper Dependencies(Action<IConfigureDependencies> from)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            from(_configureDependencies);

            return this;
        }

        public IConfigureBootstrapper ExposeContainer(Action<IContainer> exposedContainer)
        {
            if (exposedContainer == null)
            {
                throw new ArgumentNullException("exposedContainer");
            }

            _exposeContainer = exposedContainer;

            return this;
        }

        public Bootstrapper Alive()
        {
            var assemblies = _configureDependencies.SourceAssemblies;
            var types = _configureDependencies.SourceTypes;

            var assemblyLocator = new AssemblyLocator(assemblies);
            var discoverFromAssemblies = new DefaultDependencyDiscoveryTask(assemblyLocator);

            var typeLocator = new ListLocator(types);
            var discoverFromTypes = new DefaultDependencyDiscoveryTask(typeLocator);

            // add custom conventions to discovery tasks
            foreach (var convention in _conventions)
            {
                discoverFromAssemblies.AddConvention(convention);
                discoverFromTypes.AddConvention(convention);
            }

            _bootstrapper.Tasks.Add(discoverFromAssemblies);
            _bootstrapper.Tasks.Add(discoverFromTypes);

            // execute initialization tasks
            var container = _bootstrapper.Initialize();

            // expose container
            if (_exposeContainer != null)
            {
                _exposeContainer(container);
            }

            // start
            _bootstrapper.Start();

            return _bootstrapper;
        }

        public IConfigureBootstrapper Conventions(Action<ICollection<IRegistrationConvention>> configureConventions)
        {
            if (configureConventions == null)
            {
                throw new ArgumentNullException("configureConventions");
            }

            configureConventions(_conventions);

            return this;
        }

        public IConfigureBootstrapper Sequence(Action<ICollection<IInitializationTask>> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException("tasks");
            }

            tasks(_bootstrapper.Tasks);

            return this;
        }
    }
}