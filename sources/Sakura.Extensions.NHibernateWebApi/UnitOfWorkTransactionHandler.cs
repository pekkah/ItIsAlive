namespace Sakura.Extensions.NHibernateWebApi
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Autofac;

    using Sakura.Composition.Discovery;

    using global::NHibernate;

    [Priority(Priority = -100)]
    public class UnitOfWorkTransactionHandler : DelegatingHandler
    {
        private readonly ILifetimeScope lifetimeScope;

        public UnitOfWorkTransactionHandler(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(
                result =>
                {
                    object unitOfWorkValue;
                    if (result.Result.RequestMessage.Properties.TryGetValue("unitOfWork", out unitOfWorkValue))
                    {
                        var unitOfWorkScope = (ILifetimeScope)unitOfWorkValue;
                        var session = unitOfWorkScope.Resolve<ISession>();

                        if (session.Transaction == null || !session.Transaction.IsActive)
                        {
                        }
                        
                        Trace.TraceInformation("Ending transaction..");
                        if (result.Exception != null)
                        {
                            Trace.TraceError("Rolling back transaction due to error: {0}", result.Exception);
                            session.Transaction.Rollback();
                        }
                        else
                        {
                            session.Transaction.Commit();
                            Trace.TraceInformation("Transaction committed.");
                        }

                        // end unit of work
                        unitOfWorkScope.Dispose();
                    }

                    return result.Result;
                });
        }
    }
}