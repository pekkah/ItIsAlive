namespace Sakura.Extensions.NHibernateMvc
{
    using System.Web.Mvc;

    using Sakura.Extensions.NHibernateMvc.Binders;
    using Sakura.Extensions.NHibernateMvc.Filters;
    using Sakura.Framework.Fluent;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper ActivateNHibernateMvcIntegration(this ISetupBootstrapper setup)
        {
            return setup.Dependencies(d => d.Types(typeof(WorkContextBinder), typeof(WorkContextTransactionFilterAttribute)));
        }
    }
}