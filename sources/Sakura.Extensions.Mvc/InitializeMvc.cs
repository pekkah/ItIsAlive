namespace Sakura.Extensions.Mvc
{
    using System;
    using System.Collections.Generic;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.Discovery;

    [NotDiscoverable]
    public class InitializeMvc : IInitializationTask
    {
        private readonly Action configure;

        public InitializeMvc(Action configure)
        {
            this.configure = configure;
        }

        public void Execute(InitializationTaskContext context)
        {
            var builder = context.Builder;
            
            builder.RegisterFilterProvider();
            builder.RegisterModelBinderProvider();
            builder.RegisterFilterProvider();
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.Register(this.CreateStartMvcTask).As<IStartupTask>().InstancePerDependency();
        }

        public IStartupTask CreateStartMvcTask(IComponentContext context)
        {
            var lifetimeScope = context.Resolve<ILifetimeScope>();
            var globalFilters = context.Resolve<IEnumerable<Lazy<IGlobalFilter, IPriorityMetadata>>>();

            return new StartMvc(lifetimeScope, this.configure, globalFilters);
        }
    }
}