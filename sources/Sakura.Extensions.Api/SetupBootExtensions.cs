namespace Sakura.Extensions.Api
{
    using System;

    using Sakura.Extensions.Api.WebApi;
    using Sakura.Framework;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper ConfigureWebApi(
            this ISetupBootstrapper setup, Action<IWebApiRouter, ApiConfiguration> configure)
        {
            var task = new InitializeWebApi(configure);

            return setup.Task(task);
        }
    }
}