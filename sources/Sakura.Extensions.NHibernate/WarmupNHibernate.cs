namespace Sakura.Extensions.NHibernate
{
    using Autofac;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Composition.Discovery;
    using Sakura.Composition.Markers;

    [NotDiscoverable]
    public class WarmupNHibernate : IInitializationTask, ISingleInstanceDependency
    {
        public void Execute(InitializationTaskContext context)
        {
            context.Builder.RegisterType<ResolveSessionFactoryOnce>().AsImplementedInterfaces();
        }
    }
}