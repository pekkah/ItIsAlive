namespace Sakura.Extensions.NHibernateWeb.Tests.Mvc.Filters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Autofac;
    using Autofac.Features.OwnedInstances;

    using NSubstitute;

    using Sakura.Extensions.NHibernateWeb.Mvc.Filters;

    using Xunit;

    using global::NHibernate;

    public class UnitOfWorkTransactionAttributeFacts
    {
        private readonly ActionDescriptor actionDescriptor;

        private readonly UnitOfWorkTransactionAttribute actionFilter;

        private readonly ControllerContext controllerContext;

        private readonly IDictionary httpContextItems = new Dictionary<string, object>();

        private readonly ITransaction transaction;

        private readonly ISession unitOfWork;

        private Owned<ISession> ownedSession;

        public UnitOfWorkTransactionAttributeFacts()
        {
            this.unitOfWork = Substitute.For<ISession>();
            this.transaction = Substitute.For<ITransaction>();
            this.unitOfWork.Transaction.Returns(this.transaction);

            var lifetime = Substitute.For<IDisposable>();
            this.ownedSession = new Owned<ISession>(this.unitOfWork, lifetime);

            var builder = new ContainerBuilder();
            builder.RegisterInstance(this.unitOfWork).AsImplementedInterfaces().ExternallyOwned();

            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.Items.Returns(this.httpContextItems);

            this.controllerContext = new ControllerContext(
                httpContext, new RouteData(), Substitute.For<ControllerBase>());

            this.actionDescriptor = Substitute.For<ActionDescriptor>();

            this.actionFilter = new UnitOfWorkTransactionAttribute();
        }

        [Fact]
        public void should_NOT_begin_unitOfWork_if_not_in_parameters()
        {
            var parameters = new Dictionary<string, object>();

            var filterContext = new ActionExecutingContext(this.controllerContext, this.actionDescriptor, parameters);

            this.actionFilter.OnActionExecuting(filterContext);

            this.unitOfWork.DidNotReceive().BeginTransaction();
        }

        [Fact]
        public void should_begin_unitOfWork_if_in_parameters()
        {
            var parameters = new Dictionary<string, object> { { "unitOfWork", this.unitOfWork } };

            var filterContext = new ActionExecutingContext(this.controllerContext, this.actionDescriptor, parameters);

            this.actionFilter.OnActionExecuting(filterContext);

            this.unitOfWork.Received().BeginTransaction();
        }

        [Fact]
        public void should_commit_unit_of_work()
        {
            this.transaction.IsActive.Returns(true);
            this.controllerContext.HttpContext.Items["unitOfWork"] = this.ownedSession;

            var filterContext = new ActionExecutedContext(this.controllerContext, this.actionDescriptor, false, null);

            this.actionFilter.OnActionExecuted(filterContext);

            this.transaction.Received().Commit();
        }

        [Fact]
        public void should_rollback_changes_on_exception()
        {
            this.transaction.IsActive.Returns(true);
            this.controllerContext.HttpContext.Items["unitOfWork"] = this.ownedSession;

            var filterContext = new ActionExecutedContext(
                this.controllerContext, this.actionDescriptor, false, new Exception());

            this.actionFilter.OnActionExecuted(filterContext);

            this.transaction.Received().Rollback();
        }
    }
}