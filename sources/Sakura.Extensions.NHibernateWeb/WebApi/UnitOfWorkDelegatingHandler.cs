namespace Sakura.Extensions.NHibernateWeb.WebApi
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Sakura.Composition.Discovery;

    [Priority(Priority = -100)]
    public class UnitOfWorkDelegatingHandler : DelegatingHandler
    {
        private readonly WebApiUnitOfWorkHandler unitOfWorkHandler;

        public UnitOfWorkDelegatingHandler(WebApiUnitOfWorkHandler unitOfWorkHandler)
        {
            this.unitOfWorkHandler = unitOfWorkHandler;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken)
                .ContinueWith(r => this.unitOfWorkHandler.EndIfRequired(r.Result, r.Exception));
        }
    }
}