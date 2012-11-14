namespace Bootstrapper.Samples.ContactsWeb.Filters
{
    using System.Net.Http;

    using NHibernate;

    public class WebApiUnitOfWorkHandler : DelegatingHandler
    {
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var session = (ISession)request.GetDependencyScope().GetService(typeof(ISession));
            session.BeginTransaction();
            return base.SendAsync(request, cancellationToken)
                       .ContinueWith(
                           result =>
                               {
                                   if (result.Exception == null && result.Result.IsSuccessStatusCode)
                                   {
                                       session.Transaction.Commit();
                                   }
                                   else
                                   {
                                       session.Transaction.Rollback();
                                   }

                                   return result.Result;
                               });

        }
    }
}