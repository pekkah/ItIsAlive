namespace Sakura.Extensions.NHibernateMvc.Tests
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Autofac;

    using FluentAssertions;

    using Sakura.Bootstrapping;
    using Sakura.Extensions.Mvc;
    using Sakura.Extensions.NHibernateMvc.Binders;
    using Sakura.Extensions.NHibernateMvc.Filters;

    using Xunit;

    public class ConfigureBootstrapperExtensionsFacts
    {
        [Fact]
        public void should_register_unitOfWork_binder()
        {
            IContainer container = null;
            new Configure()
                .EnableMvcUnitOfWork()
                .ExposeContainer(exposed => container = exposed)
                .Start();

            var binders = container.Resolve<IEnumerable<IModelBinder>>();

            binders.Should().ContainItemsAssignableTo<UnitOfWorkBinder>();
        }

        [Fact]
        public void should_register_unitOfWork_transaction_filter()
        {
            IContainer container = null;
            new Configure()
                .EnableMvcUnitOfWork()
                .ExposeContainer(exposed => container = exposed)
                .Start();

            var globalFilters = container.Resolve<IEnumerable<IGlobalFilter>>();
            globalFilters.Should().ContainItemsAssignableTo<UnitOfWorkTransactionAttribute>();
        }
    }
}