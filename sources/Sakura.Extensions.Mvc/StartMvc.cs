namespace Sakura.Extensions.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Extensions.Mvc.Policies;
    using Sakura.Framework.Dependencies.Discovery;

    [NotDiscoverable]
    public class StartMvc : IStartupTask
    {
        private readonly Action configure;

        private readonly ILifetimeScope container;

        public StartMvc(ILifetimeScope lifetimeScope, Action configure)
        {
            this.configure = configure;
            this.container = lifetimeScope;
        }

        public void Execute()
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(this.container));

            if (HttpContext.Current != null)
            {
                if (this.configure != null)
                {
                    this.configure();
                }

                var globalFilters =
                    AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IEnumerable<IGlobalFilter>>();

                foreach (var globalFilter in globalFilters)
                {
                    GlobalFilters.Filters.Add(globalFilter);
                }
            }
        }
    }
}