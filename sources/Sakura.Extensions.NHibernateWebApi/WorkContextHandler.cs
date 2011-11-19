namespace Sakura.Extensions.NHibernateWebApi
{
    using System.Diagnostics;
    using System.Net.Http;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Microsoft.ApplicationServer.Http.Dispatcher;

    using global::NHibernate;

    using Sakura.Extensions.NHibernate;

    public class WorkContextOperationHandler : HttpOperationHandler<HttpRequestMessage, IWorkContext>
    {
        public WorkContextOperationHandler()
            : base("workContext")
        {
        }

        protected override IWorkContext OnHandle(HttpRequestMessage input)
        {
            var session = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<ISession>();
            var workContext = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IWorkContext>();
            Trace.TraceInformation("Begin transaction");
            session.BeginTransaction();
            input.Properties.Add("session", session);

            return workContext;
        }
    }
}