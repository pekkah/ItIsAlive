namespace Sakura.Extensions.NHibernateWeb.Mvc.Filters
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Features.OwnedInstances;

    using Sakura.Composition.Discovery;
    using Sakura.Extensions.NHibernate.ExtensionMethods;
    using Sakura.Extensions.Web.Mvc;

    using global::NHibernate;

    [Priority(Priority = -100)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class UnitOfWorkTransactionAttribute : ActionFilterAttribute, IGlobalFilter
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var ownedSession = this.GetUnitOfWork(filterContext.HttpContext);

            if (ownedSession == null)
            {
                return;
            }

            Trace.TraceInformation("Ending transaction...");

            if (filterContext.Exception == null)
            {
                ownedSession.Value.Transaction.Commit();
                Trace.TraceInformation("transaction committed.");
            }
            else
            {
                Trace.TraceInformation(
                    "transaction was rolled back due to error {0}.", filterContext.Exception);

                ownedSession.Value.Transaction.Rollback();
            }

            ownedSession.Dispose();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var unitOfWork =
                filterContext.ActionParameters.Values.FirstOrDefault(value => value != null && value is ISession) as
                ISession;

            if (unitOfWork == null)
            {
                return;
            }

            Trace.TraceInformation("Begin transaction");
            unitOfWork.BeginTransaction();
        }

        private Owned<ISession> GetUnitOfWork(HttpContextBase httpContext)
        {
            if (!httpContext.Items.Contains("unitOfWork"))
            {
                return null;
            }

            return httpContext.Items["unitOfWork"] as Owned<ISession>;
        }
    }
}