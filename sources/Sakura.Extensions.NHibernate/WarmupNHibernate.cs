namespace Sakura.Extensions.NHibernate
{
    using Autofac;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Discovery;

    [NotDiscoverable]
    public class WarmupNHibernate : IInitializationTask, ISingleInstanceDependency
    {
        public void Execute(InitializationTaskContext context)
        {
            context.Builder.RegisterType<ResolveSessionFactoryOnce>().AsImplementedInterfaces();
        }
    }
}