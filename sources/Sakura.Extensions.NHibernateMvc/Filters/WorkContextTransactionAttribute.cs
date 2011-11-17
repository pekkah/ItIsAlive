namespace Sakura.Extensions.NHibernateMvc.Filters
{
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    using global::NHibernate;

    using Sakura.Extensions.Mvc.Policies;
    using Sakura.Extensions.NHibernate;

    public class WorkContextTransactionAttribute : ActionFilterAttribute, IGlobalFilter
    {
        public ILifetimeScope LifetimeScope
        {
            get
            {
                return AutofacDependencyResolver.Current.RequestLifetimeScope;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var workContext = filterContext.RequestContext.HttpContext.Items["workContext"] as IWorkContext;

            if (workContext == null)
            {
                return;
            }

            var session = this.LifetimeScope.Resolve<ISession>();

            if (session == null)
            {
                return;
            }

            if (session.Transaction.IsActive)
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
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var workContext =
                filterContext.ActionParameters.Values.Where(
                    value => value != null && typeof(IWorkContext).IsAssignableFrom(value.GetType())).FirstOrDefault();

            if (workContext == null)
            {
                return;
            }

            filterContext.RequestContext.HttpContext.Items["workContext"] = workContext;

            var session = this.LifetimeScope.Resolve<ISession>();

            if (session == null)
            {
                return;
            }

            Trace.TraceInformation("Begin transaction");
            session.BeginTransaction();
        }
    }
}