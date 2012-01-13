namespace Sakura.Extensions.NHibernateWeb.Mvc
{
    using Sakura.Bootstrapping;
    using Sakura.Extensions.NHibernateWeb.Mvc.Binders;

    public static class ConfigureBootstrapperExtensions
    {
        public static IConfigureBootstrapper EnableMvcUnitOfWork(this IConfigureBootstrapper configure)
        {
            return configure.Dependencies(d => d.AssemblyOf<UnitOfWorkBinder>());
        }
    }
}