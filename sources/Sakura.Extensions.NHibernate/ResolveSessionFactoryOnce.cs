namespace Sakura.Extensions.NHibernate
{
    using System.Diagnostics;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Composition.Discovery;

    using global::NHibernate;

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