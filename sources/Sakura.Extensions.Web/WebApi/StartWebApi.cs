namespace Sakura.Extensions.Web.WebApi
{
    using System;

    using Autofac;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Composition.Discovery;

    [Hidden]
    public class StartWebApi : IStartupTask
    {
        private readonly Action<Func<ApiConfiguration>> configurationFactory;

        private readonly ILifetimeScope lifetimeScope;

        public StartWebApi(ILifetimeScope lifetimeScope, Action<Func<ApiConfiguration>> configurationFactory)
        {
            this.configurationFactory = configurationFactory;
            this.lifetimeScope = lifetimeScope;
        }

        public void Execute()
        {
            this.configurationFactory(() => new ApiConfiguration(this.lifetimeScope));
        }
    }
}