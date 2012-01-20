namespace Sakura.Extensions.NHibernateWeb.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;

    using Autofac;

    using Sakura.Extensions.NHibernate;

    public class WebApiUnitOfWorkHandler
    {
        public void BeginIfRequired(HttpRequestMessage request)
        {
            var unitOfWorkScope = this.GetUnitOfWorkScope(request.Properties);

            if (unitOfWorkScope == null)
            {
                return;
            }

            var unitOfWork = unitOfWorkScope.Resolve<IUnitOfWork>();

            Trace.TraceInformation("Begin transaction");
            unitOfWork.Begin();
        }

        public HttpResponseMessage EndIfRequired(HttpResponseMessage response, Exception exception)
        {
            var unitOfWorkScope = this.GetUnitOfWorkScope(response.RequestMessage.Properties);

            if (unitOfWorkScope == null)
            {
                return response;
            }

            var unitOfWork = unitOfWorkScope.Resolve<IUnitOfWork>();

            Trace.TraceInformation("Ending transaction..");

            // if transaction is not started or transaction was ended quit.
            if (!unitOfWork.IsActive)
            {
                Trace.TraceInformation("Transaction is not active.");
                return response;
            }

            if (exception != null)
            {
                Trace.TraceError("Rolling back transaction due to error: {0}", exception);
                unitOfWork.RollbackChanges();
            }
            else
            {
                unitOfWork.Commit();
                Trace.TraceInformation("Transaction committed.");
            }

            // end unit of work
            unitOfWorkScope.Dispose();

            return response;
        }

        private ILifetimeScope GetUnitOfWorkScope(IDictionary<string, object> properties)
        {
            object unitOfWorkScopeValue;
            if (properties.TryGetValue("unitOfWorkScope", out unitOfWorkScopeValue))
            {
                var unitOfWorkScope = (ILifetimeScope)unitOfWorkScopeValue;
                return unitOfWorkScope;
            }

            return null;
        }
    }
}