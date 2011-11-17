namespace Sakura.Extensions.Api
{
    using System;

    using Sakura.Bootstrapping.Setup;
    using Sakura.Extensions.Api.WebApi;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper ConfigureWebApi(
            this ISetupBootstrapper setup, Action<Func<ApiConfiguration>> configurationFactory)
        {
            return setup
                .Dependencies(dependencies => dependencies.AssemblyOf<StartWebApi>())
                .Tasks(manager => manager.AddTask(new InitializeWebApi(configurationFactory)));
        }
    }
}