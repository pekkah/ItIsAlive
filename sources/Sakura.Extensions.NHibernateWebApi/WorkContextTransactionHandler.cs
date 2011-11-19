namespace Sakura.Extensions.NHibernateWebApi
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Autofac;

    using global::NHibernate;

    public class WorkContextTransactionHandler : DelegatingHandler
    {
        private readonly ILifetimeScope lifetimeScope;

        public WorkContextTransactionHandler(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(
                result =>
                    {
                        object sessionValue;
                        if (result.Result.RequestMessage.Properties.TryGetValue("session", out sessionValue))
                        {
                            var session = (ISession)sessionValue;

                            if (session.Transaction != null && session.Transaction.IsActive)
                            {
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
                            }
                        }

                        return result.Result;
                    });
        }
    }
}