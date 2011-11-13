namespace Sakura.Extensions.Api
{
    using System;

    using Sakura.Bootstrapping.Configuration;
    using Sakura.Extensions.Api.WebApi;
    using Sakura.Extensions.Mvc;
    using Sakura.Framework;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper ConfigureWebApi(
            this ISetupBootstrapper setup, Action<IWebApiRouter, ApiConfiguration> configure)
        {
            var task = new InitializeWebApi(configure);
            var http = new InitializeHttpDependencies();

            return setup.Tasks(
                tasks =>
                    {
                        tasks.TryAddTask(http);
                        tasks.AddTask(task);
                    });
        }
    }
}