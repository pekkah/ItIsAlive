namespace Sakura.Extensions.NHibernateWebApi
{
    using Sakura.Bootstrapping;

    public static class ConfigurationExtensions
    {
        public static IConfigureBootstrapper EnableWebApiUnitOfWork(this IConfigureBootstrapper configure)
        {
            return configure.Dependencies(dependencies => dependencies.AssemblyOf<UnitOfWorkTransactionHandler>());
        }
    }
}