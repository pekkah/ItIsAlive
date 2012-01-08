namespace Sakura.Extensions.NHibernateMvc.Filters
{
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Autofac;

    using Sakura.Composition.Discovery;

    using global::NHibernate;

    using Sakura.Extensions.Mvc;
    using Sakura.Extensions.NHibernate;

    [Priority(Priority = -100)]
    public class UnitOfWorkTransactionAttribute : ActionFilterAttribute, IGlobalFilter
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var unitOfWorkScope = this.GetUnitOfWorkScope(filterContext.HttpContext);

            if (unitOfWorkScope == null)
            {
                return;
            }

            var session = unitOfWorkScope.Resolve<ISession>();

            if (session == null)
            {
                return;
            }

            if (session.Transaction != null && session.Transaction.IsActive)
            {
                Trace.TraceInformation("Ending transaction..");
                if (filterContext.Exception != null)
                {
                    Trace.TraceError("Rolling back transaction due to error: {0}", filterContext.Exception);
                    session.Transaction.Rollback();
                }
                else
                {
                    session.Transaction.Commit();
                    Trace.TraceInformation("Transaction committed.");
                }
            }

            unitOfWorkScope.Dispose();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object workContext =
                filterContext.ActionParameters.Values.Where(
                    value => value != null && typeof(IUnitOfWork).IsAssignableFrom(value.GetType())).FirstOrDefault();

            if (workContext == null)
            {
                return;
            }

            var session = this.GetUnitOfWorkScope(filterContext.HttpContext).Resolve<ISession>();

            if (session == null)
            {
                return;
            }

            Trace.TraceInformation("Begin transaction");
            session.BeginTransaction();
        }

        private ILifetimeScope GetUnitOfWorkScope(HttpContextBase httpContext)
        {
            if (!httpContext.Items.Contains("unitOfWork"))
            {
                return null;
            }

            return httpContext.Items["unitOfWork"] as ILifetimeScope;
        }
    }
}