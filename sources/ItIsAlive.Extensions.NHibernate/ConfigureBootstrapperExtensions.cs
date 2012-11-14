namespace ItIsAlive.Extensions.NHibernate
{
    using System;

    using Autofac.Builder;

    using Bootstrapping;

    using global::NHibernate;
    using global::NHibernate.Cfg;

    public static class ConfigureBootstrapperExtensions
    {
        public static IConfigureBootstrapper ConfigureNHibernate(
            this IConfigureBootstrapper configureBootstrapper,
            Func<Configuration> configure,
            Action<IRegistrationBuilder<ISession, SimpleActivatorData, SingleRegistrationStyle>>
                modifySessionRegistration = null)
        {
            var initializationTask = new RegisterNHibernate(configure);
            var registerSession = new RegisterSession(modifySessionRegistration);

            return configureBootstrapper.Dependencies(d => d.AssemblyOf<RegisterNHibernate>()).Sequence(
                tasks =>
                    {
                        tasks.Add(initializationTask);
                        tasks.Add(registerSession);
                    });
        }

        public static IConfigureBootstrapper WarmupNHibernate(this IConfigureBootstrapper configure)
        {
            return configure.Sequence(manager => manager.Add(new WarmupNHibernate()));
        }
    }
}