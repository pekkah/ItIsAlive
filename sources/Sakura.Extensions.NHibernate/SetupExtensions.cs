namespace Sakura.Extensions.NHibernate
{
    using System;

    using Autofac.Builder;

    using Sakura.Bootstrapping;

    using global::NHibernate;
    using global::NHibernate.Cfg;

    public static class SetupExtensions
    {
        public static IConfigureBootstrapper ConfigureNHibernate(
            this IConfigureBootstrapper configure, 
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

        public static IConfigureBootstrapper WarmupNHibernate(this IConfigureBootstrapper configure)
        {
            return configure.Tasks(manager => manager.TryAddTask(new WarmupNHibernate()));
        }
    }
}