namespace Sakura.Extensions.NHibernateMvc
{
    using Sakura.Bootstrapping;
    using Sakura.Composition;
    using Sakura.Extensions.NHibernateMvc.Binders;

    public static class SetupExtensions
    {
        public static IConfigureBootstrapper EnableMvcUnitOfWork(this IConfigureBootstrapper configure)
        {
            return configure.Dependencies(d => d.AssemblyOf<UnitOfWorkBinder>());
        }
    }
}