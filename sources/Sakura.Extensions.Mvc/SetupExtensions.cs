namespace Sakura.Extensions.Mvc
{
    using System;

    using Sakura.Bootstrapping.Setup;
    using Sakura.Extensions.Mvc.Conventions;

    public static class SetupExtensions
    {
        public static ISetupBootstrapper ConfigureMvc(this ISetupBootstrapper setup, Action configure)
        {
            var initializeMvc = new InitializeMvc(configure);

            return
                setup.Dependencies(dependencies => dependencies.AssemblyOf<StartMvc>()).Tasks(
                    tasks => tasks.AddTask(initializeMvc)).Conventions(
                        policies =>
                            {
                                policies.Add(new ControllersAsSelf());
                                policies.Add(new ModelBinderConvention());
                                policies.Add(new GlobalFilterConvention());
                            });
        }
    }
}