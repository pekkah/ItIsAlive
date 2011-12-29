namespace Sakura.Extensions.NHibernateWebApi
{
    using System.Diagnostics;
    using System.Net.Http;

    using Autofac;

    using Microsoft.ApplicationServer.Http.Dispatcher;

    using global::NHibernate;

    using Sakura.Extensions.NHibernate;
    using Sakura.Framework.Dependencies.Discovery;

    [Priority(Priority = -100)]
    public class UnitOfWorkOperationHandler : HttpOperationHandler<HttpRequestMessage, IUnitOfWork>
    {
        private readonly ILifetimeScope lifetimeScope;

        public UnitOfWorkOperationHandler(ILifetimeScope lifetimeScope)
            : base("workContext")
        {
            this.lifetimeScope = lifetimeScope;
        }

        protected override IUnitOfWork OnHandle(HttpRequestMessage input)
        {
            var unitOfWorkScope = this.lifetimeScope.BeginLifetimeScope("unitOfWork");

            var session = unitOfWorkScope.Resolve<ISession>();
            var workContext = unitOfWorkScope.Resolve<IUnitOfWork>();

            Trace.TraceInformation("Begin transaction");
            session.BeginTransaction();

            // store unit of work scope
            input.Properties.Add("unitOfWork", unitOfWorkScope);

            return workContext;
        }
    }
}