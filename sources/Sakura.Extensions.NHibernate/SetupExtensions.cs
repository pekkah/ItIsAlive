namespace Sakura.Extensions.NHibernate
{
    using System;

    using Autofac.Builder;

    using global::NHibernate;
    using global::NHibernate.Cfg;

    using Sakura.Bootstrapping.Setup;

    public static class SetupExtensions
    {
        public static ISetupBootstrapper ConfigureNHibernate(
            this ISetupBootstrapper setup, 
            Func<Configuration> configure, 
            Action<IRegistrationBuilder<ISession, SimpleActivatorData, SingleRegistrationStyle>> modifySessionRegistration = null)
        {
            var initializationTask = new RegisterNHibernate(configure);
            var registerSession = new RegisterSession(modifySessionRegistration);

            return setup.Dependencies(d => d.AssemblyOf<RegisterNHibernate>()).Tasks(
                tasks =>
                    {
                        tasks.AddTask(initializationTask);
                        tasks.AddTask(registerSession);
                    });
        }

        public static ISetupBootstrapper WarmupNHibernate(this ISetupBootstrapper setup)
        {
            return setup.Tasks(manager => manager.TryAddTask(new WarmupNHibernate()));
        }
    }
}