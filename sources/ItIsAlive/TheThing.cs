namespace ItIsAlive
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Autofac;
    using Tasks;

    public sealed class TheThing : IDisposable
    {
        private IContainer _container;

        public TheThing()
        {
            Sequence = new Collection<IInitializationTask>();
        }

        public Collection<IInitializationTask> Sequence { get; private set; }

        public void Dispose()
        {
            Shutdown();
        }

        public IContainer Initialize()
        {
            var builder = new ContainerBuilder();

            var context = new InitializationTaskContext(builder);

            foreach (IInitializationTask task in Sequence)
            {
                task.Execute(context);
            }

            return _container = builder.Build();
        }

        public void Shutdown()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
        }

        public void Start()
        {
            var tasks = _container.Resolve<IEnumerable<IStartupTask>>();

            foreach (IStartupTask task in tasks)
            {
                task.Execute();
            }
        }
    }
}