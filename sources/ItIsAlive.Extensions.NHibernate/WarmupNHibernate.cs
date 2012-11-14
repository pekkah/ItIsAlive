namespace ItIsAlive.Extensions.NHibernate
{
    using Autofac;

    using Bootstrapping.Tasks;

    using Composition.Discovery;
    using Composition.Markers;

    [Hidden]
    public class WarmupNHibernate : IInitializationTask, ISingleInstanceDependency
    {
        public void Execute(InitializationTaskContext context)
        {
            context.Builder.RegisterType<ResolveSessionFactoryOnce>().AsImplementedInterfaces();
        }
    }
}