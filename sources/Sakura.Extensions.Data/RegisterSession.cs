namespace Sakura.Extensions.Data
{
    using System.Diagnostics;

    using Autofac;

    using NHibernate;

    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Discovery;
    using Sakura.Framework.Tasks.Types;

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