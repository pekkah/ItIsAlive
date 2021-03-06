namespace ItIsAlive.Extensions.NHibernate
{
    using System;
    using System.Diagnostics;

    using Autofac;
    using Autofac.Builder;

    using Bootstrapping.Tasks;

    using Composition.Discovery;
    using Composition.Markers;

    using global::NHibernate;

    [Hidden]
    public class RegisterSession : IInitializationTask, ISingleInstanceDependency
    {
        private readonly Action<IRegistrationBuilder<ISession, SimpleActivatorData, SingleRegistrationStyle>>
            modifySessionRegistration;

        public RegisterSession(
            Action<IRegistrationBuilder<ISession, SimpleActivatorData, SingleRegistrationStyle>>
                modifySessionRegistration)
        {
            this.modifySessionRegistration = modifySessionRegistration;
        }

        public void Execute(InitializationTaskContext context)
        {
            // register session so that each lifetime scope will have their own instance
            var registration =
                context.Builder.Register(GetSession).As<ISession>().InstancePerLifetimeScope().OnActivated(
                    handler => Trace.TraceInformation("Session activated")).OnRelease(
                        release => Trace.TraceInformation("Session released."));

            // allow modification of the session registration
            if (modifySessionRegistration != null)
            {
                modifySessionRegistration(registration);
            }
        }

        private ISession GetSession(IComponentContext componentContext)
        {
            var factory = componentContext.Resolve<ISessionFactory>();

            return factory.OpenSession();
        }
    }
}