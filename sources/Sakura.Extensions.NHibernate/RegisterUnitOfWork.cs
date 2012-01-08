namespace Sakura.Extensions.NHibernate
{
    using System.Diagnostics;

    using Autofac;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Composition.Markers;

    using global::NHibernate;

    public class RegisterUnitOfWork : IInitializationTask, ISingleInstanceDependency
    {
        public void Execute(InitializationTaskContext context)
        {
            context.Builder.Register(this.CreateContext).As<IUnitOfWork>().InstancePerLifetimeScope().OnActivated(
                handler => Trace.TraceInformation("Work context activated")).OnRelease(
                    release => Trace.TraceInformation("Work context released."));
        }

        private UnitOfWork CreateContext(IComponentContext componentContext)
        {
            var session = componentContext.Resolve<ISession>();

            return new UnitOfWork(session);
        }
    }
}