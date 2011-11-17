namespace Sakura.Extensions.Mvc
{
    using System;

    using Sakura.Bootstrapping.Setup;
    using Sakura.Extensions.Mvc.Policies;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper ConfigureMvc(this ISetupBootstrapper setup, Action configure)
        {
            var initializeMvc = new InitializeMvc(configure);

            return setup.Dependencies(dependencies => dependencies.AssemblyOf<StartMvc>()).Tasks(
                tasks => tasks.AddTask(initializeMvc)).RegistrationPolicies(
                    policies =>
                            {
                                policies.Add(new ControllersAsSelf());
                                policies.Add(new ModelBinderPolicy());
                                policies.Add(new GlobalFilterPolicy());
                            });
        }
    }
}