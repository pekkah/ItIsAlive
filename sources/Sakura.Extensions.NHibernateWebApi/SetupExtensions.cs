namespace Sakura.Extensions.NHibernateWebApi
{
    using Sakura.Bootstrapping;
    using Sakura.Composition;

    public static class SetupExtensions
    {
        public static IConfigureBootstrapper EnableWebApiUnitOfWork(this IConfigureBootstrapper configure)
        {
            return configure.Dependencies(dependencies => dependencies.AssemblyOf<UnitOfWorkTransactionHandler>());
        }
    }
}