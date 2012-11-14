namespace ItIsAlive
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Autofac;
    using Composition;
    using Composition.Discovery;
    using Tasks;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class It : IIt
    {
        private readonly ConfigureDependencies _configureDependencies;

        private readonly List<IRegistrationConvention> _conventions;
        private readonly TheThing _theThing;

        private Action<IContainer> _exposeContainer;

        protected It()
        {
            _theThing = new TheThing();
            _configureDependencies = new ConfigureDependencies();
            _conventions = new List<IRegistrationConvention>();
        }

        public static IIt Is
        {
            get
            {
                return new It();
            }    
        }

        public IIt Composed(Action<IConfigureDependencies> from)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            from(_configureDependencies);

            return this;
        }

        public IIt ExposeContainer(Action<IContainer> exposedContainer)
        {
            if (exposedContainer == null)
            {
                throw new ArgumentNullException("exposedContainer");
            }

            _exposeContainer = exposedContainer;

            return this;
        }

        public TheThing Alive()
        {
            IEnumerable<Assembly> assemblies = _configureDependencies.SourceAssemblies;
            IEnumerable<Type> types = _configureDependencies.SourceTypes;

            var assemblyLocator = new AssemblyLocator(assemblies);
            var discoverFromAssemblies = new DefaultDependencyDiscoveryTask(assemblyLocator);

            var typeLocator = new ListLocator(types);
            var discoverFromTypes = new DefaultDependencyDiscoveryTask(typeLocator);

            // add custom conventions to discovery tasks
            foreach (IRegistrationConvention convention in _conventions)
            {
                discoverFromAssemblies.AddConvention(convention);
                discoverFromTypes.AddConvention(convention);
            }

            _theThing.Sequence.Add(discoverFromAssemblies);
            _theThing.Sequence.Add(discoverFromTypes);

            // execute initialization tasks
            IContainer container = _theThing.Initialize();

            // expose container
            if (_exposeContainer != null)
            {
                _exposeContainer(container);
            }

            // start
            _theThing.Start();

            return _theThing;
        }

        public IIt Using(Action<ICollection<IRegistrationConvention>> configureConventions)
        {
            if (configureConventions == null)
            {
                throw new ArgumentNullException("configureConventions");
            }

            configureConventions(_conventions);

            return this;
        }

        public IIt Sequence(Action<ICollection<IInitializationTask>> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException("tasks");
            }

            tasks(_theThing.Sequence);

            return this;
        }
    }
}