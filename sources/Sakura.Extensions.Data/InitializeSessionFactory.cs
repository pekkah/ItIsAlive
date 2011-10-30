namespace Sakura.Extensions.Data
{
    using System;

    using Autofac;

    using NHibernate;
    using NHibernate.Cfg;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Tasks;

    [NotDiscoverable]
    public class InitializeSessionFactory : IInitializationTask
    {
        private readonly Func<Configuration> configure;

        public InitializeSessionFactory(Func<Configuration> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException("configure");
            }

            this.configure = configure;
        }

        public void Execute(InitializationTaskContext context)
        {
            var builder = context.Builder;

            builder.Register(c => this.configure().BuildSessionFactory()).As<ISessionFactory>().SingleInstance();
        }
    }
}