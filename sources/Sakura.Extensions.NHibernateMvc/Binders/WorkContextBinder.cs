namespace Sakura.Extensions.NHibernateMvc.Binders
{
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Sakura.Extensions.NHibernate;

    [ModelBinderType(typeof(IWorkContext))]
    public class WorkContextBinder : IModelBinder
    {
        public ILifetimeScope Lifetime { get; set; }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return this.Lifetime.Resolve<IWorkContext>();
        }
    }
}