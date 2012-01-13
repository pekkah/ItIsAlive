namespace Sakura.Extensions.NHibernateWeb.WebApi
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Autofac;

    using Sakura.Composition.Discovery;
    using Sakura.Extensions.NHibernate;

    [Priority(Priority = -100)]
    public class UnitOfWorkTransactionHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(
                result =>
                    {
                        object unitOfWorkScopeValue;
                        if (result.Result.RequestMessage.Properties.TryGetValue("unitOfWorkScope", out unitOfWorkScopeValue))
                        {
                            var unitOfWorkScope = (ILifetimeScope)unitOfWorkScopeValue;
                            var unitOfWork = unitOfWorkScope.Resolve<IUnitOfWork>();

                            Trace.TraceInformation("Ending transaction..");

                            // if transaction is not started or transaction was ended quit.
                            if (!unitOfWork.IsActive)
                            {
                                Trace.TraceInformation("Transaction is not active.");
                                return result.Result;
                            }

                            if (result.Exception != null)
                            {
                                Trace.TraceError("Rolling back transaction due to error: {0}", result.Exception);
                                unitOfWork.RollbackChanges();
                            }
                            else
                            {
                                unitOfWork.Commit();
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