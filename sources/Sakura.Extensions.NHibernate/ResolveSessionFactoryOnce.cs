namespace Sakura.Extensions.NHibernate
{
    using System.Diagnostics;

    using global::NHibernate;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.Discovery;

    [NotDiscoverable]
    public class ResolveSessionFactoryOnce : IStartupTask
    {
        private readonly ISessionFactory sessionFactory;

        public ResolveSessionFactoryOnce(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            Trace.TraceInformation("Warming up session factory");
        }
    }
}