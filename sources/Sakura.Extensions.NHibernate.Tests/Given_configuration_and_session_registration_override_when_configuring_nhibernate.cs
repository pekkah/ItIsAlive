namespace Sakura.Extensions.NHibernate.Tests
{
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using Machine.Fakes;
    using Machine.Specifications;

    using Sakura.Bootstrapping.Setup;
    using Sakura.Extensions.NHibernate.Tests.DatabaseModel;

    using global::NHibernate;
    using global::NHibernate.Cfg;
    using global::NHibernate.Dialect;
    using global::NHibernate.Driver;
    using global::NHibernate.Mapping.ByCode;

    public class Given_configuration_and_session_registration_override_when_started: WithSubject<Setup>
    {
        private static IContainer container;

        private Establish context = () => Subject.Dependencies(d => d.AssemblyOf<RegisterNHibernate>()).ExposeContainer(
            exposed => container = exposed).ConfigureNHibernate(
                () =>
                    {
                        var config = new Configuration();

                        config.DataBaseIntegration(
                            db =>
                                {
                                    db.Dialect<SQLiteDialect>();
                                    db.Driver<SQLite20Driver>();
                                    db.SchemaAction = SchemaAutoAction.Recreate;
                                    db.ConnectionString = "Data Source=:memory:;Version=3;New=True;";
                                }).SetProperty(Environment.CurrentSessionContextClass, "thread_static");

                        var mapper = new ConventionModelMapper();

                        // filter entities
                        var baseEntityType = typeof(AbstractEntity);
                        mapper.IsEntity(
                            (t, declared) =>
                            baseEntityType.IsAssignableFrom(t) && baseEntityType != t && !t.IsInterface);
                        mapper.IsRootEntity((t, declared) => baseEntityType.Equals(t.BaseType));

                        // override base properties
                        mapper.Class<AbstractEntity>(
                            map => map.Id(x => x.Id, m => m.Generator(Generators.GuidComb)));

                        mapper.BeforeMapProperty += (modelinspector, member, propertycustomizer) =>
                            {
                                if (member.LocalMember.Name == "Name")
                                {
                                    propertycustomizer.Unique(true);
                                }
                            };

                        // compile
                        var mapping =
                            mapper.CompileMappingFor(
                                typeof(Person).Assembly.GetExportedTypes().Where(
                                    type => typeof(AbstractEntity).IsAssignableFrom(type)));

                        // use mappings
                        config.AddMapping(mapping);

                        return config;
                    },
                sessionOverride => sessionOverride.InstancePerMatchingLifetimeScope("test"));

        private Because of = () => Subject.Start();

        private It should_register_session_as_set_in_override = () =>
            {
                var registration =
                    container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(ISession))).
                        SingleOrDefault();

                registration.ShouldNotBeNull();
                registration.Lifetime.ShouldBeOfType<MatchingScopeLifetime>();
            };
    }
}