namespace Fugu.Extensions.Data
{
    using Autofac;

    using Fugu.Framework.Tasks;

    using NHibernate;

    public class RegisterNHibernate : IInitializationTask
    {
        public void Execute(InitializationTaskContext context)
        {
            var builder = context.Builder;

            builder.Register(c => new SessionFactoryFactory().Create()).As<ISessionFactory>().SingleInstance();
            builder.Register(c => c.Resolve<ISessionFactory>().OpenSession()).As<ISession>().InstancePerDependency();
        }
    }
}