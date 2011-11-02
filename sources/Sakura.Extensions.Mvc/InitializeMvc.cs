namespace Sakura.Extensions.Mvc
{
    using System;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Sakura.Extensions.Mvc.Web;
    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Tasks;

    [NotDiscoverable]
    public class InitializeMvc : IInitializationTask
    {
        private readonly Action<IWebRouter> configure;

        public InitializeMvc(Action<IWebRouter> configure)
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
            builder.RegisterInstance(this.configure);
        }
    }
}