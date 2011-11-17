namespace Sakura.Extensions.NHibernateWebApi
{
    using Sakura.Bootstrapping.Setup;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper EnableWebApiWorkContext(this ISetupBootstrapper setup)
        {
            return setup;
        }
    }
}