namespace Sakura.Extensions.NHibernateMvc.Filters
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Autofac;

    using Sakura.Composition.Discovery;
    using Sakura.Extensions.Mvc;
    using Sakura.Extensions.NHibernate;

    [Priority(Priority = -100)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class UnitOfWorkTransactionAttribute : ActionFilterAttribute, IGlobalFilter
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var unitOfWorkScope = this.GetUnitOfWorkScope(filterContext.HttpContext);

            if (unitOfWorkScope == null)
            {
                return;
            }

            var unitOfWork = unitOfWorkScope.Resolve<IUnitOfWork>();

            if (unitOfWork == null)
            {
                return;
            }

            if (!unitOfWork.IsActive)
            {
                return;
            }

            Trace.TraceInformation("Ending transaction..");
            if (filterContext.Exception != null)
            {
                Trace.TraceError("Rolling back transaction due to error: {0}", filterContext.Exception);
                unitOfWork.RollbackChanges();
            }
            else
            {
                unitOfWork.Commit();
                Trace.TraceInformation("Transaction committed.");
            }

            unitOfWorkScope.Dispose();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var unitOfWork =
                filterContext.ActionParameters.Values.FirstOrDefault(value => value != null && value is IUnitOfWork) as
                IUnitOfWork;

            if (unitOfWork == null)
            {
                return;
            }

            Trace.TraceInformation("Begin transaction");
            unitOfWork.Begin();
        }

        private ILifetimeScope GetUnitOfWorkScope(HttpContextBase httpContext)
        {
            if (!httpContext.Items.Contains("unitOfWorkScope"))
            {
                return null;
            }

            return httpContext.Items["unitOfWorkScope"] as ILifetimeScope;
        }
    }
}