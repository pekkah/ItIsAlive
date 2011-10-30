namespace Sakura.Extensions.Api
{
    using System;

    using Autofac;

    using Sakura.Extensions.Api.WebApi;
    using Sakura.Framework;
    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Tasks;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper ConfigureWebApi(
            this ISetupBootstrapper setup, Action<IRouting, ApiConfiguration> configure)
        {
            var task = new InitializeWebApi(configure);

            return setup.Task(task);
        }
    }

    [NotDiscoverable]
    public class InitializeWebApi : IInitializationTask
    {
        private readonly Action<IRouting, ApiConfiguration> configure;

        public InitializeWebApi(Action<IRouting, ApiConfiguration> configure)
        {
            this.configure = configure;
        }

        public void Execute(InitializationTaskContext context)
        {
            var builder = context.Builder;

            builder.RegisterInstance(this.configure).AsSelf();
        }
    }

    public class StartWebApi : IStartupTask
    {
        private readonly Action<IRouting, ApiConfiguration> configure;

        private readonly ILifetimeScope lifetimeScope;

        private readonly IRouting routing;

        public StartWebApi(Action<IRouting, ApiConfiguration> configure, ILifetimeScope lifetimeScope, IRouting routing)
        {
            this.configure = configure;
            this.lifetimeScope = lifetimeScope;
            this.routing = routing;
        }

        public void Execute()
        {
            var configuration = new ApiConfiguration(this.lifetimeScope);

            this.configure(this.routing, configuration);
        }
    }
}