namespace Sakura.Extensions.NHibernate
{
    using System.Diagnostics;

    using Autofac;

    using Sakura.Bootstrapping.Tasks;

    using global::NHibernate;

    public class RegisterUnitOfWork : IInitializationTask
    {
        public void Execute(InitializationTaskContext context)
        {
            context.Builder.Register(this.CreateContext).As<IUnitOfWork>().InstancePerLifetimeScope().OnActivated(
                handler => Trace.TraceInformation("UnitOfWork activated")).OnRelease(
                    release => Trace.TraceInformation("UnitOfWork released."));
        }

        private UnitOfWork CreateContext(IComponentContext componentContext)
        {
            var session = componentContext.Resolve<ISession>();

            return new UnitOfWork(session);
        }
    }
}