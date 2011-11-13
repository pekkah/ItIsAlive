namespace Sakura.Extensions.Data
{
    using System;

    using NHibernate.Cfg;

    using Sakura.Bootstrapping.Configuration;

    public static class SetupExtensions
    {
        public static ISetupBootstrapper ConfigureNHibernate(
            this ISetupBootstrapper setup, Func<Configuration> configure)
        {
            var initializationTask = new RegisterNHibernate(configure);

            return setup.Dependencies(d => d.AssemblyOf<RegisterNHibernate>()).Tasks(tasks =>
                {
                    tasks.AddTask(initializationTask);
                    tasks.AddTask(new RegisterSession());
                });
        }
    }
}