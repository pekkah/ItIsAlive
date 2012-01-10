namespace Sakura.Extensions.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Composition.Discovery;

    [Hidden]
    public class StartMvc : IStartupTask
    {
        private readonly Action configure;

        private readonly ILifetimeScope container;

        private readonly IEnumerable<Lazy<IGlobalFilter, IPriorityMetadata>> globalFilters;

        public StartMvc(ILifetimeScope lifetimeScope, Action configure, IEnumerable<Lazy<IGlobalFilter, IPriorityMetadata>> globalFilters)
        {
            this.configure = configure;
            this.globalFilters = globalFilters;
            this.container = lifetimeScope;
        }

        public void Execute()
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(this.container));

            this.configure();

            foreach (var filter in this.globalFilters.OrderBy(f => f.Metadata.Priority))
            {
                GlobalFilters.Filters.Add(filter.Value);
            }
        }
    }
}