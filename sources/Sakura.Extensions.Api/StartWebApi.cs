namespace Sakura.Extensions.Api
{
    using System;

    using Autofac;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Extensions.Api.WebApi;

    public class StartWebApi : IStartupTask
    {
        private readonly Action<IWebApiRouter, ApiConfiguration> configure;

        private readonly ILifetimeScope lifetimeScope;

        private readonly IWebApiRouter webApiRouter;

        public StartWebApi(Action<IWebApiRouter, ApiConfiguration> configure, ILifetimeScope lifetimeScope, IWebApiRouter webApiRouter)
        {
            this.configure = configure;
            this.lifetimeScope = lifetimeScope;
            this.webApiRouter = webApiRouter;
        }

        public void Execute()
        {
            var configuration = new ApiConfiguration(this.lifetimeScope);

            this.configure(this.webApiRouter, configuration);
        }
    }
}