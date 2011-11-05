﻿using Sakura.Framework.Samples.ContactsWeb.App_Start;

using WebActivator;

[assembly: PostApplicationStartMethod(typeof(Boot), "Start")]
[assembly: ApplicationShutdownMethod(typeof(Boot), "Shutdown")]

namespace Sakura.Framework.Samples.ContactsWeb.App_Start
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;

    using NHibernate.Cfg;
    using NHibernate.Dialect;
    using NHibernate.Driver;
    using NHibernate.Mapping.ByCode;

    using Sakura.Extensions.Data;
    using Sakura.Extensions.Data.Model;
    using Sakura.Extensions.Mvc;
    using Sakura.Extensions.Mvc.Web;
    using Sakura.Framework.Samples.ContactsWeb.Database;

    public class Boot
    {
        private static Bootstrapper bootstrapper;

        public static void Start()
        {
            bootstrapper = new SetupBoot()
                .DependenciesFrom(Assembly.GetExecutingAssembly())
                .ConfigureMvc(ConfigureRoutes)
                .ConfigureNHibernate(ConfigureNHibernate)
                .Start();
        }

        public static void Shutdown()
        {
            bootstrapper.Shutdown();
        }

        private static Configuration ConfigureNHibernate()
        {

            var config = new Configuration();

            var appDataFolder = HttpContext.Current.Server.MapPath("~/App_Data");
            var databaseFilePath = Path.Combine(appDataFolder, "Data.sdf");

            var connectionString = string.Format("Data Source={0}; Persist Security Info=False;", databaseFilePath);

            config.DataBaseIntegration(
                db =>
                    {
                        db.Dialect<MsSqlCe40Dialect>();
                        db.Driver<SqlServerCeDriver>();
                        db.SchemaAction = SchemaAutoAction.Recreate;
                        db.ConnectionString = connectionString;
                        db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                    });

            var mapper = new ConventionModelMapper();

            // filter entities
            var baseEntityType = typeof(AbstractEntity);
            mapper.IsEntity(
                (t, declared) => baseEntityType.IsAssignableFrom(t) && baseEntityType != t && !t.IsInterface);
            mapper.IsRootEntity((t, declared) => baseEntityType.Equals(t.BaseType));

            // override base properties
            mapper.Class<AbstractEntity>(map => { map.Id(x => x.Id, m => m.Generator(Generators.GuidComb)); });

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

        private static void ConfigureRoutes(IWebRouter router)
        {
            router.IgnoreRoute("{resource}.axd/{*pathInfo}");

            router.MapRoute(
                "Default",
                // Route name
                "{controller}/{action}/{id}",
                // URL with parameters
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                } // Parameter defaults
                );
        }
    }
}