namespace Sakura.Extensions.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Sakura.Extensions.Mvc.Policies;
    using Sakura.Extensions.Mvc.Web;
    using Sakura.Framework.Tasks.Types;

    public class StartMvc : IStartupTask
    {
        private readonly Action<IWebRouter> configure;

        private readonly ILifetimeScope container;

        private readonly IWebRouter router;

        public StartMvc(ILifetimeScope container, IWebRouter router, Action<IWebRouter> configure)
        {
            this.container = container;
            this.configure = configure;
            this.router = router;
        }

        public void Execute()
        {
            this.configure(this.router);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(this.container));

            var globalFilters = this.container.Resolve<IEnumerable<IGlobalFilter>>();

            foreach (var globalFilter in globalFilters)
            {
                GlobalFilters.Filters.Add(globalFilter);
            }
        }
    }
}