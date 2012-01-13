namespace Sakura.Extensions.NHibernateWeb.Tests.Mvc.Binders
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Autofac;

    using FluentAssertions;

    using NSubstitute;

    using Sakura.Extensions.NHibernate;
    using Sakura.Extensions.NHibernateWeb.Mvc.Binders;

    using Xunit;

    public class UnitOfWorkBinderFacts
    {
        private readonly UnitOfWorkBinder binder;

        private readonly ControllerContext controllerContext;

        private readonly ModelBindingContext modelBindingContext;

        private readonly ILifetimeScope rootScope;

        private readonly IUnitOfWork unitOfWork;

        public UnitOfWorkBinderFacts()
        {
            this.unitOfWork = Substitute.For<IUnitOfWork>();

            /* Cannot substitute ILifetimeScope as the Resolve<T>
             * methods are extension methods which cannot be mocked. */
            var builder = new ContainerBuilder();
            builder.RegisterInstance(this.unitOfWork).AsImplementedInterfaces();
            var container = builder.Build();
            this.rootScope = container;

            // setup mock http context
            var httpContext = Substitute.For<HttpContextBase>();

            this.modelBindingContext = new ModelBindingContext();
            this.controllerContext = new ControllerContext(
                httpContext, new RouteData(), Substitute.For<ControllerBase>());

            this.binder = new UnitOfWorkBinder { Lifetime = this.rootScope };
        }

        [Fact]
        public void should_NOT_begin_unitOfWork()
        {
            this.binder.BindModel(this.controllerContext, this.modelBindingContext).As<IUnitOfWork>().DidNotReceive().
                Begin();
        }

        [Fact]
        public void should_begin_unitOfWorkScope()
        {
            this.binder.BindModel(this.controllerContext, this.modelBindingContext);

            var scope = this.controllerContext.HttpContext.Items["unitOfWorkScope"] as ILifetimeScope;

            scope.Should().NotBeNull();
        }

        [Fact]
        public void should_return_unitOfWork()
        {
            this.binder.BindModel(this.controllerContext, this.modelBindingContext).Should().BeSameAs(this.unitOfWork);
        }
    }
}