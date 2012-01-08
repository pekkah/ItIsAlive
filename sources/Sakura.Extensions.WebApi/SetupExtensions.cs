namespace Sakura.Extensions.WebApi
{
    using System;

    using Sakura.Bootstrapping;
    using Sakura.Composition;
    using Sakura.Extensions.WebApi.Conventions;
    using Sakura.Extensions.WebApi.WebApi;

    public static class SetupExtensions
    {
        public static IConfigureBootstrapper ConfigureWebApi(
            this IConfigureBootstrapper configure, Action<Func<ApiConfiguration>> configurationFactory)
        {
            return configure.Dependencies(dependencies => dependencies.AssemblyOf<StartWebApi>()).Conventions(
                conventions =>
                    {
                        conventions.Add(new ServiceContractConvention());
                        conventions.Add(new DelegatingHandlerConvention());
                        conventions.Add(new HttpOperationHandlerConvention());
                    }).Tasks(manager => manager.AddTask(new InitializeWebApi(configurationFactory)));
        }
    }
}