namespace Sakura.Extensions.NHibernateWeb.Mvc.Binders
{
    using System.Diagnostics;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Features.OwnedInstances;
    using Autofac.Integration.Mvc;

    using global::NHibernate;

    [ModelBinderType(typeof(ISession))]
    public class UnitOfWorkBinder : IModelBinder
    {
        public ILifetimeScope RootScope { get; set; }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var ownedSession = this.RootScope.Resolve<Owned<ISession>>();

            // transaction is started OnActionExecuting of the action filter

            controllerContext.HttpContext.Items.Add("unitOfWork", ownedSession);

            return ownedSession.Value;
        }
    }
}