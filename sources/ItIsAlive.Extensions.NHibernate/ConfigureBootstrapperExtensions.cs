namespace ItIsAlive.Extensions.NHibernate
{
    using System;
    using Autofac.Builder;
    using global::NHibernate;
    using global::NHibernate.Cfg;

    public static class ConfigureBootstrapperExtensions
    {
        public static IIt ConfigureNHibernate(
            this IIt it,
            Func<Configuration> configure,
            Action<IRegistrationBuilder<ISession, SimpleActivatorData, SingleRegistrationStyle>>
                modifySessionRegistration = null)
        {
            var initializationTask = new RegisterNHibernate(configure);
            var registerSession = new RegisterSession(modifySessionRegistration);

            return it.Composed(d => d.AssemblyOf<RegisterNHibernate>()).Sequence(
                tasks =>
                    {
                        tasks.Add(initializationTask);
                        tasks.Add(registerSession);
                    });
        }

        public static IIt WarmupNHibernate(this IIt configure)
        {
            return configure.Sequence(manager => manager.Add(new WarmupNHibernate()));
        }
    }
}