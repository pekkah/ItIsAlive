namespace ItIsAlive.Samples.ContactsWeb.Filters
{
    using System.Web.Mvc;

    using NHibernate;

    public class MvcUnitOfWorkAttribute : ActionFilterAttribute
    {
        public ISession Session { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            Session.BeginTransaction();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                if (Session.Transaction != null && Session.Transaction.IsActive)
                {
                    Session.Transaction.Commit();
                }
            }
            else
            {
                if (Session.Transaction != null && Session.Transaction.IsActive)
                {
                    Session.Transaction.Rollback();
                }
            }

            base.OnResultExecuted(filterContext);
        }
    }
}