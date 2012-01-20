namespace Sakura.Extensions.NHibernateWeb.WebApi
{
    using System.Diagnostics;
    using System.Net.Http;

    using Autofac;

    using Microsoft.ApplicationServer.Http.Dispatcher;

    using Sakura.Composition.Discovery;
    using Sakura.Extensions.NHibernate;

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

            // store unit of work scope
            input.Properties.Add("unitOfWorkScope", unitOfWorkScope);

            var unitOfWork = unitOfWorkScope.Resolve<IUnitOfWork>();
            
            Trace.TraceInformation("Begin transaction");
            unitOfWork.Begin();

            return unitOfWork;
        }
    }
}