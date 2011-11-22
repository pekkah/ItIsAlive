namespace Sakura.Extensions.NHibernateWebApi
{
    using Sakura.Bootstrapping.Setup;

    public static class SetupExtensions
    {
        public static ISetupBootstrapper EnableWebApiWorkContext(this ISetupBootstrapper setup)
        {
            var registerHandlers = new RegisterWorkContextHandler();
            return setup.Tasks(manager => manager.TryAddTask(registerHandlers))
                .Dependencies(dependencies => dependencies.AssemblyOf<WorkContextTransactionHandler>());
        }
    }
}