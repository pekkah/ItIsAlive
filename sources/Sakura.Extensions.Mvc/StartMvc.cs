namespace Sakura.Extensions.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.Discovery;

    [NotDiscoverable]
    public class StartMvc : IStartupTask
    {
        private readonly Action configure;

        private readonly ILifetimeScope container;

        private readonly IEnumerable<IGlobalFilter> globalFilters;

        public StartMvc(ILifetimeScope lifetimeScope, Action configure, IEnumerable<IGlobalFilter> globalFilters)
        {
            this.configure = configure;
            this.globalFilters = globalFilters;
            this.container = lifetimeScope;
        }

        public void Execute()
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(this.container));

            this.configure();
            foreach (IGlobalFilter globalFilter in this.globalFilters)
            {
                GlobalFilters.Filters.Add(globalFilter);
            }
        }
    }
}