namespace Sakura.Extensions.Data
{
    using System.Diagnostics;

    using Autofac;

    using NHibernate;

    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Tasks.Types;

    public class RegisterWorkContext : IInitializationTask, ISingleInstanceDependency
    {
        public void Execute(InitializationTaskContext context)
        {
            context.Builder.Register(this.CreateContext).As<IWorkContext>().InstancePerLifetimeScope()
                .OnActivated(
                handler => Trace.TraceInformation("Work context activated")).OnRelease(
                    release => Trace.TraceInformation("Work context released."));
        }

        private WorkContext CreateContext(IComponentContext componentContext)
        {
            var session = componentContext.Resolve<ISession>();

            return new WorkContext(session);
        }
    }
}