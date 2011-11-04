namespace Sakura.Extensions.Mvc
{
    using System;

    using Sakura.Extensions.Mvc.Web;
    using Sakura.Framework;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper ConfigureMvc(
            this ISetupBootstrapper setup, Action<IWebRouter> configure)
        {
            var initializeMvc = new InitializeMvc(configure);

            return setup
                .DependenciesFrom(typeof(InitializeHttpDependencies))
                .TryTask(new InitializeHttpDependencies())
                .Task(initializeMvc);
        }
    }
}