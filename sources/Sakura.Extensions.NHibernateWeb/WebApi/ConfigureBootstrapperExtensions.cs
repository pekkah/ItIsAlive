namespace Sakura.Extensions.NHibernateWeb.WebApi
{
    using Sakura.Bootstrapping;

    public static class ConfigureBootstrapperExtensions
    {
        public static IConfigureBootstrapper EnableWebApiUnitOfWork(this IConfigureBootstrapper configure)
        {
            return configure.Dependencies(from => from.AssemblyOf<UnitOfWorkParameterHandler>());
        }
    }
}