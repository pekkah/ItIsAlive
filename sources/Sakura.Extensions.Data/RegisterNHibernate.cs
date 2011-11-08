namespace Sakura.Extensions.Data
{
    using Autofac;

    using NHibernate;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Tasks;
    using Sakura.Framework.Tasks.Types;

    public class RegisterNHibernate : IInitializationTask, ISingleInstanceDependency
    {
        public void Execute(InitializationTaskContext context)
        {
            var builder = context.Builder;
            builder.Register(c => c.Resolve<ISessionFactory>().OpenSession()).As<ISession>().InstancePerDependency();
        }
    }
}