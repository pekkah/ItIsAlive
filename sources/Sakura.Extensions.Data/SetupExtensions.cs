namespace Sakura.Extensions.Data
{
    using System;

    using NHibernate.Cfg;

    using Sakura.Framework.Fluent;

    public static class SetupExtensions
    {
        public static ISetupBootstrapper ConfigureNHibernate(
            this ISetupBootstrapper setup, Func<Configuration> configure)
        {
            var initializationTask = new RegisterNHibernate(configure);

            return setup.Dependencies(d => d.AssemblyOf<RegisterNHibernate>()).Task(initializationTask);
        }
    }
}