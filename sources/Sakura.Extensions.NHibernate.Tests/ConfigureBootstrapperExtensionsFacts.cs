namespace Sakura.Extensions.NHibernate.Tests
{
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using Sakura.Bootstrapping;
    using Sakura.Extensions.NHibernate.Tests.DatabaseModel;

    using Xunit;

    using global::NHibernate;
    using global::NHibernate.Cfg;
    using global::NHibernate.Dialect;
    using global::NHibernate.Driver;
    using global::NHibernate.Mapping.ByCode;

    public class ConfigureBootstrapperExtensionsFacts
    {
        private static IContainer container;

        private readonly IConfigureBootstrapper configure;

        public ConfigureBootstrapperExtensionsFacts()
        {
            this.configure = new Configure().Dependencies(d => d.AssemblyOf<RegisterNHibernate>()).ExposeContainer(
                exposed => container = exposed).ConfigureNHibernate(this.ConfigureNHibernate);
        }

        [Fact]
        public void should_add_warmup_task()
        {
            var bootstrapper = this.configure.WarmupNHibernate().Start();
            bootstrapper.Tasks.Should().Contain(t => t.GetType() == typeof(WarmupNHibernate));
        }

        [Fact]
        public void should_configure_session_factory()
        {
            this.configure.Start();

            var sessionFactory = container.Resolve<ISessionFactory>();

            sessionFactory.Should().NotBeNull();
        }

        [Fact]
        public void should_register_session_as_current_lifetime_scope()
        {
            this.configure.Start();

            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(ISession))).SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }

        [Fact]
        public void should_register_session_factory()
        {
            this.configure.Start();

            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(ISessionFactory))).SingleOrDefault(
                    );

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<RootScopeLifetime>();
        }

        private Configuration ConfigureNHibernate()
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
                (t, declared) => baseEntityType.IsAssignableFrom(t) && baseEntityType != t && !t.IsInterface);
            mapper.IsRootEntity((t, declared) => baseEntityType == t.BaseType);

            // override base properties
            mapper.Class<AbstractEntity>(map => map.Id(x => x.Id, m => m.Generator(Generators.GuidComb)));

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
        }
    }
}