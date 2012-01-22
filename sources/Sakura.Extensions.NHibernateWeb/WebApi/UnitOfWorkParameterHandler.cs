namespace Sakura.Extensions.NHibernateWeb.WebApi
{
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;

    using Autofac;
    using Autofac.Features.OwnedInstances;

    using Microsoft.ApplicationServer.Http.Description;
    using Microsoft.ApplicationServer.Http.Dispatcher;

    using Sakura.Extensions.Web.WebApi;

    using global::NHibernate;

    public class UnitOfWorkParameterHandler : HttpOperationHandler<HttpRequestMessage, ISession>, IApplies<HttpOperationDescription>
    {
        private readonly ILifetimeScope rootScope;

        public UnitOfWorkParameterHandler(ILifetimeScope rootScope)
            : base("session")
        {
            this.rootScope = rootScope;
        }

        protected override ISession OnHandle(HttpRequestMessage input)
        {
            var ownedSession = this.rootScope.Resolve<Owned<ISession>>();

            Trace.TraceInformation("Begin transaction");
            ownedSession.Value.BeginTransaction();

            input.Properties.Add("unitOfWork", ownedSession);

            return ownedSession.Value;
        }

        public bool To(HttpOperationDescription to)
        {
            return to.InputParameters.Any(p => p.IsAssignableFromParameter<ISession>());
        }
    }
}