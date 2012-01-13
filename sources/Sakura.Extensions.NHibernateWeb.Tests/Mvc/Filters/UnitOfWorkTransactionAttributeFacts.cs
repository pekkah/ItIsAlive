namespace Sakura.Extensions.NHibernateWeb.Tests.Mvc.Filters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Autofac;

    using NSubstitute;

    using Sakura.Extensions.NHibernate;
    using Sakura.Extensions.NHibernateWeb.Mvc.Filters;

    using Xunit;

    public class UnitOfWorkTransactionAttributeFacts
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly UnitOfWorkTransactionAttribute actionFilter;

        private readonly ControllerContext controllerContext;

        private readonly ActionDescriptor actionDescriptor;

        private readonly ILifetimeScope unitOfWorkScope;

        private readonly IDictionary httpContextItems = new Dictionary<string,object>();

        public UnitOfWorkTransactionAttributeFacts()
        {
            this.unitOfWork = Substitute.For<IUnitOfWork>();

            var builder = new ContainerBuilder();
            builder.RegisterInstance(this.unitOfWork).AsImplementedInterfaces();
            this.unitOfWorkScope = builder.Build().BeginLifetimeScope("unitOfWorkScope");

            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.Items.Returns(this.httpContextItems);

            this.controllerContext = new ControllerContext(
                httpContext, new RouteData(), Substitute.For<ControllerBase>());

            this.actionDescriptor = Substitute.For<ActionDescriptor>();

            this.actionFilter = new UnitOfWorkTransactionAttribute();
        }

        [Fact]
        public void should_begin_unitOfWork_if_in_parameters()
        {
            var parameters = new Dictionary<string, object> { { "unitOfWorkScope", this.unitOfWork } };

            var filterContext = new ActionExecutingContext(
                this.controllerContext,
                this.actionDescriptor,
                parameters);

            this.actionFilter.OnActionExecuting(filterContext);

            this.unitOfWork.Received().Begin();
        }

        [Fact]
        public void should_NOT_begin_unitOfWork_if_not_in_parameters()
        {
            var parameters = new Dictionary<string, object>();

            var filterContext = new ActionExecutingContext(
                this.controllerContext,
                this.actionDescriptor,
                parameters);

            this.actionFilter.OnActionExecuting(filterContext);

            this.unitOfWork.DidNotReceive().Begin();
        }

        [Fact]
        public void should_commit_unit_of_work()
        {
            this.unitOfWork.IsActive.Returns(true);
            this.controllerContext.HttpContext.Items["unitOfWorkScope"] = this.unitOfWorkScope;

            var filterContext = new ActionExecutedContext(
                this.controllerContext,
                this.actionDescriptor,
                false,
                null);

            this.actionFilter.OnActionExecuted(filterContext);

            this.unitOfWork.Received().Commit();
        }

        [Fact]
        public void should_rollback_changes_on_exception()
        {
            this.unitOfWork.IsActive.Returns(true);
            this.controllerContext.HttpContext.Items["unitOfWorkScope"] = this.unitOfWorkScope;

            var filterContext = new ActionExecutedContext(
                this.controllerContext,
                this.actionDescriptor,
                false,
                new Exception());

            this.actionFilter.OnActionExecuted(filterContext);

            this.unitOfWork.Received().RollbackChanges();
        }
    }
}