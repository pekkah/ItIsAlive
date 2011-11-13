namespace Sakura.Extensions.NHibernate
{
    using System.Diagnostics;

    using Autofac;

    using global::NHibernate;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Discovery;

    [NotDiscoverable]
    public class RegisterSession : IInitializationTask, ISingleInstanceDependency
    {
        public void Execute(InitializationTaskContext context)
        {
            // register session so that each lifetime scope will have their own instance
            context.Builder.Register(this.GetSession).As<ISession>().InstancePerLifetimeScope().OnActivated(
                handler => Trace.TraceInformation("Session activated")).OnRelease(
                    release => Trace.TraceInformation("Session released."));
        }

        private ISession GetSession(IComponentContext componentContext)
        {
            var factory = componentContext.Resolve<ISessionFactory>();

            return factory.OpenSession();
        }
    }
}