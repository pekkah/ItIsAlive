namespace Sakura.Extensions.NHibernate
{
    using System;
    using System.Diagnostics;

    using Autofac;
    using Autofac.Builder;

    using global::NHibernate;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Discovery;

    [NotDiscoverable]
    public class RegisterSession : IInitializationTask, ISingleInstanceDependency
    {
        private readonly Action<IRegistrationBuilder<ISession, SimpleActivatorData, SingleRegistrationStyle>> modifySessionRegistration;

        public RegisterSession(
            Action<IRegistrationBuilder<ISession, SimpleActivatorData, SingleRegistrationStyle>> modifySessionRegistration)
        {
            this.modifySessionRegistration = modifySessionRegistration;
        }

        public void Execute(InitializationTaskContext context)
        {
            // register session so that each lifetime scope will have their own instance
            var registration =
                context.Builder.Register(this.GetSession).As<ISession>().InstancePerLifetimeScope().OnActivated(
                    handler => Trace.TraceInformation("Session activated")).OnRelease(
                        release => Trace.TraceInformation("Session released."));

            // allow modification of the session registration
            if (this.modifySessionRegistration != null)
            {
                this.modifySessionRegistration(registration);
            }
        }

        private ISession GetSession(IComponentContext componentContext)
        {
            var factory = componentContext.Resolve<ISessionFactory>();

            return factory.OpenSession();
        }
    }
}