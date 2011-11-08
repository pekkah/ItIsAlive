namespace Sakura.Extensions.Data
{
    using System;

    using NHibernate.Cfg;

    using Sakura.Framework;
    using Sakura.Framework.Fluent;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper ConfigureNHibernate(this ISetupBootstrapper setup, Func<Configuration> configure)
        {
            var initializationTask = new InitializeSessionFactory(configure);

            return setup.Dependencies(d => d.AssemblyOf<RegisterNHibernate>()).Task(initializationTask);
        }
    }
}