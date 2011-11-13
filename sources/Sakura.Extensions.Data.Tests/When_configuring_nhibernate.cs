namespace Sakura.Extensions.Data.Tests
{
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Dialect;
    using NHibernate.Driver;
    using NHibernate.Mapping.ByCode;

    using NUnit.Framework;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Configuration;
    using Sakura.Extensions.Data.Tests.DatabaseModel;
    using Sakura.Framework;

    [TestFixture]
    [Explicit("This test fixture executes against inmemory database (sqllite)")]
    public class When_configuring_nhibernate
    {
        private Bootstrapper bootstrapper;

        private IContainer container;

        [SetUp]
        public void Setup()
        {
            this.bootstrapper =
                new Setup().Dependencies(d => d.AssemblyOf<RegisterNHibernate>()).ExposeContainer(
                    exposed => this.container = exposed).ConfigureNHibernate(
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
                                    map => { map.Id(x => x.Id, m => m.Generator(Generators.GuidComb)); });

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
                            }).Start();
        }

        [Test]
        public void should_configure_session_factory()
        {
            var sessionFactory = this.container.Resolve<ISessionFactory>();

            sessionFactory.Should().NotBeNull();
        }

        [Test]
        public void should_register_session_factory()
        {
            var registration =
                this.container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(ISessionFactory))).
                    SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<RootScopeLifetime>();
        }
    }
}