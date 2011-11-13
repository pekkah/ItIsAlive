namespace Sakura.Extensions.NHibernate
{
    using System;
    using System.Diagnostics;

    using Autofac;

    using global::NHibernate;
    using global::NHibernate.Cfg;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Discovery;

    [NotDiscoverable]
    public class RegisterNHibernate : IInitializationTask, ISingleInstanceDependency
    {
        private readonly Func<Configuration> configure;

        public RegisterNHibernate(Func<Configuration> configure)
        {
            this.configure = configure;
        }

        public void Execute(InitializationTaskContext context)
        {
            var builder = context.Builder;

            // register session factory as singleton
            builder.Register(this.CreateSessionFactory).As<ISessionFactory>().SingleInstance().OnActivated(
                handler => Trace.TraceInformation("Session factory activated")).OnRelease(
                    release => Trace.TraceInformation("Session factory released."));

            builder.Register(this.GetStatelessSession).As<IStatelessSession>().InstancePerLifetimeScope().OnActivated(
                handler => Trace.TraceInformation("Stateless session factory activated")).OnRelease(
                    release => Trace.TraceInformation("Stateless session released."));
        }

        private ISessionFactory CreateSessionFactory(IComponentContext componentContext)
        {
            return this.configure().BuildSessionFactory();
        }

        private IStatelessSession GetStatelessSession(IComponentContext componentContext)
        {
            var factory = componentContext.Resolve<ISessionFactory>();

            return factory.OpenStatelessSession();
        }
    }
}