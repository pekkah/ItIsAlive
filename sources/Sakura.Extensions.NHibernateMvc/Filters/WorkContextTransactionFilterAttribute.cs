namespace Sakura.Extensions.NHibernateMvc.Filters
{
    using System.Linq;
    using System.Web.Mvc;

    using Autofac;

    using NHibernate;

    using Sakura.Extensions.Data;
    using Sakura.Extensions.Mvc.Policies;

    public class WorkContextTransactionFilterAttribute : ActionFilterAttribute, IGlobalFilter
    {
        public ILifetimeScope LifetimeScope
        {
            get;
            set;
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
                if (filterContext.Exception != null)
                {
                    session.Transaction.Rollback();
                }
                else
                {
                    session.Transaction.Commit();
                }

            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var workContext =
                filterContext.ActionParameters.Values.Where(
                    value =>
                        {
                            if (value == null)
                            {
                                return false;
                            }

                            return typeof(IWorkContext).IsAssignableFrom(value.GetType());
                        }).FirstOrDefault();

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

            session.BeginTransaction();
        }
    }
}