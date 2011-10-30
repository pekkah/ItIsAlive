namespace Fugu.Extensions.Data
{
    using System.Linq;

    using Fugu.Extensions.Data.Model;

    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Dialect;
    using NHibernate.Mapping.ByCode;

    public class SessionFactoryFactory
    {
        public ISessionFactory Create()
        {
            var cfg = new Configuration();
            cfg.DataBaseIntegration(
                db =>
                {
                    db.ConnectionString =
                        "Server=tcp:localhost;Database=DataSample;User ID=sqluser;Password=sqluser;Trusted_Connection=False;Encrypt=False;";
                    db.Dialect<MsSql2008Dialect>();
                    db.BatchSize = 250;
                    db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                    db.SchemaAction = SchemaAutoAction.Update;
                }).SessionFactory().GenerateStatistics();

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

            mapper.BeforeMapProperty += this.OnBeforeMapProperty;

            // compile
            var mapping =
                mapper.CompileMappingFor(
                    typeof(AbstractEntity).Assembly.GetExportedTypes().Where(
                        type => typeof(AbstractEntity).IsAssignableFrom(type)));

            // use mappings
            cfg.AddMapping(mapping);

            // build
            return cfg.BuildSessionFactory();
        }

        private void OnBeforeMapProperty(
            IModelInspector modelinspector, PropertyPath member, IPropertyMapper propertycustomizer)
        {
            if (member.LocalMember.Name == "Name")
            {
                propertycustomizer.Unique(true);
            }
        }
    }
}