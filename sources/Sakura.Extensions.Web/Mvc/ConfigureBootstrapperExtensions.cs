namespace Sakura.Extensions.Web.Mvc
{
    using System;

    using Sakura.Bootstrapping;
    using Sakura.Extensions.Web.Mvc.Conventions;

    public static class ConfigureBootstrapperExtensions
    {
        public static IConfigureBootstrapper ConfigureMvc(
            this IConfigureBootstrapper configureBootstrapper, Action configure)
        {
            var initializeMvc = new InitializeMvc(configure);

            return
                configureBootstrapper.Dependencies(dependencies => dependencies.AssemblyOf<StartMvc>()).Tasks(
                    tasks => tasks.Add(initializeMvc)).Conventions(
                        policies =>
                            {
                                policies.Add(new ControllersAsSelf());
                                policies.Add(new ModelBinderConvention());
                                policies.Add(new GlobalFilterConvention());
                            });
        }
    }
}