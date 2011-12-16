namespace Sakura.Extensions.NHibernateWebApi
{
    using System.Diagnostics;
    using System.Net.Http;

    using Autofac;

    using Microsoft.ApplicationServer.Http.Dispatcher;

    using global::NHibernate;

    using Sakura.Extensions.NHibernate;

    public class WorkContextOperationHandler : HttpOperationHandler<HttpRequestMessage, IWorkContext>
    {
        private readonly ILifetimeScope lifetimeScope;

        public WorkContextOperationHandler(ILifetimeScope lifetimeScope)
            : base("workContext")
        {
            this.lifetimeScope = lifetimeScope;
        }

        protected override IWorkContext OnHandle(HttpRequestMessage input)
        {
            var unitOfWorkScope = this.lifetimeScope.BeginLifetimeScope("unitOfWork");

            var session = unitOfWorkScope.Resolve<ISession>();
            var workContext = unitOfWorkScope.Resolve<IWorkContext>();

            Trace.TraceInformation("Begin transaction");
            session.BeginTransaction();

            // store unit of work scope
            input.Properties.Add("unitOfWork", unitOfWorkScope);

            return workContext;
        }
    }
}