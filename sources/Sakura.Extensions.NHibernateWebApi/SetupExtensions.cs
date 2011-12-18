namespace Sakura.Extensions.NHibernateWebApi
{
    using Sakura.Bootstrapping.Setup;

    public static class SetupExtensions
    {
        public static ISetupBootstrapper EnableWebApiWorkContext(this ISetupBootstrapper setup)
        {
            return setup.Dependencies(dependencies => dependencies.AssemblyOf<WorkContextTransactionHandler>());
        }
    }
}