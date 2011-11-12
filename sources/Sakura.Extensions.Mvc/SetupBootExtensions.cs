namespace Sakura.Extensions.Mvc
{
    using System;

    using Sakura.Extensions.Mvc.Policies;
    using Sakura.Extensions.Mvc.Web;
    using Sakura.Framework.Fluent;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper ConfigureMvc(this ISetupBootstrapper setup, Action<IWebRouter> configure)
        {
            var initializeMvc = new InitializeMvc(configure);

            return setup.Dependencies(d => d.AssemblyOf<InitializeHttpDependencies>()).Tasks(
                tasks =>
                {
                    tasks.AddTask(new InitializeHttpDependencies());
                    tasks.AddTask(initializeMvc);
                }).RegistrationPolicies(
                        policies =>
                        {
                            policies.Add(new ControllersAsSelf());
                            policies.Add(new ModelBinderPolicy());
                            policies.Add(new GlobalFilterPolicy());
                        });
        }
    }
}