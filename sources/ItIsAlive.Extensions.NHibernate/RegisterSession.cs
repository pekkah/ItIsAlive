namespace ItIsAlive.Extensions.NHibernate
{
    using System;
    using System.Diagnostics;
    using Autofac;
    using Autofac.Builder;
    using Composition.Discovery;
    using Composition.Markers;
    using Tasks;
    using global::NHibernate;

    [Hidden]
    public class RegisterSession : IInitializationTask, ISingleInstanceDependency
    {
        private readonly Action<IRegistrationBuilder<ISession, SimpleActivatorData, SingleRegistrationStyle>>
            _modifySessionRegistration;

        public RegisterSession(
            Action<IRegistrationBuilder<ISession, SimpleActivatorData, SingleRegistrationStyle>>
                modifySessionRegistration)
        {
            this._modifySessionRegistration = modifySessionRegistration;
        }

        public void Execute(InitializationTaskContext context)
        {
            // register session so that each lifetime scope will have their own instance
            IRegistrationBuilder<ISession, SimpleActivatorData, SingleRegistrationStyle> registration =
                context.Builder.Register(GetSession).As<ISession>().InstancePerLifetimeScope().OnActivated(
                    handler => Trace.TraceInformation("Session activated")).OnRelease(
                        release => Trace.TraceInformation("Session released."));

            // allow modification of the session registration
            if (_modifySessionRegistration != null)
            {
                _modifySessionRegistration(registration);
            }
        }

        private ISession GetSession(IComponentContext componentContext)
        {
            var factory = componentContext.Resolve<ISessionFactory>();

            return factory.OpenSession();
        }
    }
}