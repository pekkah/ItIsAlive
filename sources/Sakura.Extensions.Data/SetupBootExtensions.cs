namespace Sakura.Extensions.Data
{
    using System;

    using NHibernate.Cfg;

    using Sakura.Framework;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper ConfigureNHibernate(this ISetupBootstrapper setup, Func<Configuration> configure)
        {
            var initializationTask = new InitializeSessionFactory(configure);

            return setup.DependenciesFrom(typeof(RegisterNHibernate)).Task(initializationTask);
        }
    }
}