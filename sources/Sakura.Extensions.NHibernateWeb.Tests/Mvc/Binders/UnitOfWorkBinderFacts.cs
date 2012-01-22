namespace Sakura.Extensions.NHibernateWeb.Tests.Mvc.Binders
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Autofac;

    using FluentAssertions;

    using NSubstitute;

    using Sakura.Extensions.NHibernateWeb.Mvc.Binders;

    using Xunit;

    using global::NHibernate;

    public class UnitOfWorkBinderFacts
    {
        private readonly UnitOfWorkBinder binder;

        private readonly ControllerContext controllerContext;

        private readonly ModelBindingContext modelBindingContext;

        private readonly ILifetimeScope rootScope;

        private readonly ISession unitOfWork;

        public UnitOfWorkBinderFacts()
        {
            this.unitOfWork = Substitute.For<ISession>();

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

            this.binder = new UnitOfWorkBinder { RootScope = this.rootScope };
        }

        [Fact]
        public void should_return_unitOfWork()
        {
            this.binder.BindModel(this.controllerContext, this.modelBindingContext)
                .Should().BeSameAs(this.unitOfWork);
        }
    }
}