namespace Bootstrapper.Extensions.NHibernate
{
    using System;
    using System.Diagnostics;

    using Bootstrapping.Tasks;

    using Composition.Discovery;

    using global::NHibernate;

    [Hidden]
    public class ResolveSessionFactoryOnce : IStartupTask
    {
        public ResolveSessionFactoryOnce(ISessionFactory sessionFactory)
        {
            if (sessionFactory == null)
            {
                throw new ArgumentNullException("sessionFactory");
            }
        }

        public void Execute()
        {
            Trace.TraceInformation("Warming up session factory");
        }
    }
}