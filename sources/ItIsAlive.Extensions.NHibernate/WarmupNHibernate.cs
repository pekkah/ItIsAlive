namespace ItIsAlive.Extensions.NHibernate
{
    using Autofac;
    using Composition.Discovery;
    using Composition.Markers;
    using Tasks;

    [Hidden]
    public class WarmupNHibernate : IInitializationTask, ISingleInstanceDependency
    {
        public void Execute(InitializationTaskContext context)
        {
            context.Builder.RegisterType<ResolveSessionFactoryOnce>().AsImplementedInterfaces();
        }
    }
}