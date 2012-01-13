namespace Sakura.Extensions.Web.WebApi
{
    using System;

    using Sakura.Bootstrapping;
    using Sakura.Extensions.Web.WebApi.Conventions;

    public static class ConfigureBootstrapperExtensions
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
                    }).Tasks(manager => manager.Add(new InitializeWebApi(configurationFactory)));
        }
    }
}