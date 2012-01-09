namespace Sakura.Extensions.NHibernateMvc
{
    using Sakura.Bootstrapping;
    using Sakura.Extensions.NHibernateMvc.Binders;

    public static class ConfigureBootstrapperExtensions
    {
        public static IConfigureBootstrapper EnableMvcUnitOfWork(this IConfigureBootstrapper configure)
        {
            return configure.Dependencies(d => d.AssemblyOf<UnitOfWorkBinder>());
        }
    }
}