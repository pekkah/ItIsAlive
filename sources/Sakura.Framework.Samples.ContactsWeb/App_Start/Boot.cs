using Sakura.Framework.Samples.ContactsWeb.App_Start;

using WebActivator;

[assembly: PostApplicationStartMethod(typeof(Boot), "Start")]

namespace Sakura.Framework.Samples.ContactsWeb.App_Start
{
    using System.Linq;

    using NHibernate.Cfg;
    using NHibernate.Dialect;
    using NHibernate.Driver;
    using NHibernate.Mapping.ByCode;

    using Sakura.Extensions.Data;
    using Sakura.Extensions.Data.Model;
    using Sakura.Framework.Samples.ContactsWeb.Database;

    public class Boot
    {
        private static Bootstrapper bootstrapper;

        public static void Start()
        {
            bootstrapper = new SetupBoot().ConfigureNHibernate(ConfigureNHibernate).Start();
        }

        private static Configuration ConfigureNHibernate()
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
            mapper.IsRootEntity((t, declared) => baseEntityType.Equals(t.BaseType));

            // override base properties
            mapper.Class<AbstractEntity>(map =>
            {
                map.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
            });

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
                    typeof(Contact).Assembly.GetExportedTypes().Where(
                        type => typeof(AbstractEntity).IsAssignableFrom(type)));

            // use mappings
            config.AddMapping(mapping);

            return config;
        }
    }
}