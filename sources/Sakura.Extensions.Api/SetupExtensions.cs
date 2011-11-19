namespace Sakura.Extensions.Api
{
    using System;

    using Sakura.Bootstrapping.Setup;
    using Sakura.Extensions.Api.Conventions;
    using Sakura.Extensions.Api.WebApi;

    public static class SetupExtensions
    {
        public static ISetupBootstrapper ConfigureWebApi(
            this ISetupBootstrapper setup, Action<Func<ApiConfiguration>> configurationFactory)
        {
            return setup.Dependencies(dependencies => dependencies.AssemblyOf<StartWebApi>()).Conventions(
                conventions =>
                    {
                        conventions.Add(new ServiceContractConvention());
                        conventions.Add(new DelegatingHandlerConvention());
                    }).Tasks(manager => manager.AddTask(new InitializeWebApi(configurationFactory)));
        }
    }
}