namespace Sakura.Extensions.Mvc
{
    using System;

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
            builder.Register(
                componentContext => new StartMvc(componentContext.Resolve<ILifetimeScope>(), this.configure)).
                AsImplementedInterfaces();
        }
    }
}