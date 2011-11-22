namespace Sakura.Extensions.NHibernateMvc
{
    using Sakura.Bootstrapping.Setup;
    using Sakura.Extensions.NHibernateMvc.Binders;
    using Sakura.Extensions.NHibernateMvc.Filters;

    public static class SetupExtensions
    {
        public static ISetupBootstrapper EnableMvcWorkContext(this ISetupBootstrapper setup)
        {
            return setup.Dependencies(d => d.Types(typeof(WorkContextBinder), typeof(WorkContextTransactionAttribute)));
        }
    }
}