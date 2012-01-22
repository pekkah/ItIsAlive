namespace Sakura.Extensions.NHibernateWeb.WebApi
{
    using System.Diagnostics;
    using System.Net.Http;

    using Autofac.Features.OwnedInstances;

    using Sakura.Composition.Discovery;
    using Sakura.Extensions.NHibernate.ExtensionMethods;

    using global::NHibernate;

    [Priority(Priority = -100)]
    public class UnitOfWorkHandler : DelegatingHandler
    {
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return
                base.SendAsync(request, cancellationToken).ContinueWith(
                    result =>
                        {
                            var response = result.Result;

                            if (response.RequestMessage.Properties.ContainsKey("unitOfWork"))
                            {
                                var ownedSession = response.RequestMessage.Properties["unitOfWork"] as Owned<ISession>;

                                if (ownedSession == null)
                                {
                                    return result.Result;
                                }

                                Trace.TraceInformation("Ending transaction...");

                                if (ownedSession.Value.IsActive())
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        ownedSession.Value.Transaction.Commit();
                                        Trace.TraceInformation("transaction committed.");
                                    }
                                    else
                                    {
                                        Trace.TraceInformation(
                                            "transaction was rolled back due to response statuc code {0}.",
                                            response.StatusCode);

                                        ownedSession.Value.Transaction.Rollback();
                                    }
                                }
                                else
                                {
                                    Trace.TraceInformation("Cannot commit transaction as its not active.");
                                }

                                ownedSession.Dispose();
                            }

                        return result.Result;
                    });
        }
    }
}