namespace Sakura.Extensions.NHibernateMvc
{
    using Sakura.Bootstrapping.Setup;
    using Sakura.Extensions.NHibernateMvc.Binders;

    public static class SetupExtensions
    {
        public static ISetupBootstrapper EnableMvcUnitOfWork(this ISetupBootstrapper setup)
        {
            return setup.Dependencies(d => d.AssemblyOf<UnitOfWorkBinder>());
        }
    }
}