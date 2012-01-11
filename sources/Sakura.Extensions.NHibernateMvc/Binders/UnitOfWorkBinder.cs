namespace Sakura.Extensions.NHibernateMvc.Binders
{
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Sakura.Extensions.NHibernate;

    [ModelBinderType(typeof(IUnitOfWork))]
    public class UnitOfWorkBinder : IModelBinder
    {
        public ILifetimeScope Lifetime { get; set; }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var unitOfWorkScope = this.Lifetime.BeginLifetimeScope("unitOfWork");
            controllerContext.HttpContext.Items["unitOfWorkScope"] = unitOfWorkScope;
            
            return unitOfWorkScope.Resolve<IUnitOfWork>();
        }
    }
}