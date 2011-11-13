namespace Sakura.Extensions.NHibernate
{
    using System;

    using global::NHibernate.Cfg;

    using Sakura.Bootstrapping.Setup;

    public static class SetupExtensions
    {
        public static ISetupBootstrapper ConfigureNHibernate(
            this ISetupBootstrapper setup, Func<Configuration> configure)
        {
            var initializationTask = new RegisterNHibernate(configure);

            return setup.Dependencies(d => d.AssemblyOf<RegisterNHibernate>()).Tasks(
                tasks =>
                    {
                        tasks.AddTask(initializationTask);
                        tasks.AddTask(new RegisterSession());
                    });
        }
    }
}