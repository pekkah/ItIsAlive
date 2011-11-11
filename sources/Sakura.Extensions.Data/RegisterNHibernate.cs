namespace Sakura.Extensions.Data
{
    using System;
    using System.Diagnostics;

    using Autofac;

    using NHibernate;
    using NHibernate.Cfg;

    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Discovery;
    using Sakura.Framework.Tasks.Types;

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

            // register session so that each lifetime scope will have their own instance
            builder.Register(this.GetSession).As<ISession>().InstancePerLifetimeScope().OnActivated(
                handler => Trace.TraceInformation("Session activated")).OnRelease(
                    release => Trace.TraceInformation("Session released."));

            builder.Register(this.GetStatelessSession).As<IStatelessSession>().InstancePerLifetimeScope().OnActivated(
                handler => Trace.TraceInformation("Stateless session factory activated")).OnRelease(
                    release => Trace.TraceInformation("Stateless session released."));
        }

        private ISessionFactory CreateSessionFactory(IComponentContext componentContext)
        {
            return this.configure().BuildSessionFactory();
        }

        private ISession GetSession(IComponentContext componentContext)
        {
            var factory = componentContext.Resolve<ISessionFactory>();

            return factory.OpenSession();
        }

        private IStatelessSession GetStatelessSession(IComponentContext componentContext)
        {
            var factory = componentContext.Resolve<ISessionFactory>();

            return factory.OpenStatelessSession();
        }
    }
}