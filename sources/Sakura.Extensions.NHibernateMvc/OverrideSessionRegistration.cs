namespace Sakura.Extensions.NHibernateMvc
{
    using System.Diagnostics;

    using Autofac;
    using Autofac.Integration.Mvc;

    using NHibernate;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Discovery;

    [NotDiscoverable]
    public class OverrideSessionRegistration : IInitializationTask, ISingleInstanceDependency
    {
        public void Execute(InitializationTaskContext context)
        {
            // register session so that each lifetime scope will have their own instance
            context.Builder.Register(this.GetSession).As<ISession>().InstancePerMatchingLifetimeScope("httpRequest").OnActivated(
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