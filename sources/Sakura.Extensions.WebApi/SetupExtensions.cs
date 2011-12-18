namespace Sakura.Extensions.WebApi
{
    using System;

    using Sakura.Bootstrapping.Setup;
    using Sakura.Extensions.WebApi.Conventions;
    using Sakura.Extensions.WebApi.WebApi;

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
                        conventions.Add(new HttpOperationHandlerConvention());
                    }).Tasks(manager => manager.AddTask(new InitializeWebApi(configurationFactory)));
        }
    }
}