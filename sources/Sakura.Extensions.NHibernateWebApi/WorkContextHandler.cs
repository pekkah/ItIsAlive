namespace Sakura.Extensions.NHibernateWebApi
{
    using System.Net.Http;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Microsoft.ApplicationServer.Http.Dispatcher;

    using Sakura.Extensions.NHibernate;

    public class WorkContextOperationHandler : HttpOperationHandler<HttpRequestMessage, IWorkContext>
    {
        public WorkContextOperationHandler()
            : base("workContext")
        {
        }

        protected override IWorkContext OnHandle(HttpRequestMessage input)
        {
            return AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IWorkContext>();
        }
    }
}