namespace Sakura.Extensions.Data
{
    using Autofac;

    using NHibernate;

    using Sakura.Framework.Tasks;

    public class RegisterNHibernate : IInitializationTask
    {
        public void Execute(InitializationTaskContext context)
        {
            var builder = context.Builder;
            builder.Register(c => c.Resolve<ISessionFactory>().OpenSession()).As<ISession>().InstancePerDependency();
        }
    }
}