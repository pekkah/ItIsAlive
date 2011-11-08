using Sakura.Framework.Samples.ContactsWeb.App_Start;

using WebActivator;

[assembly: PostApplicationStartMethod(typeof(Boot), "Start")]
[assembly: ApplicationShutdownMethod(typeof(Boot), "Shutdown")]

namespace Sakura.Framework.Samples.ContactsWeb.App_Start
{
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using NHibernate.Cfg;
    using NHibernate.Dialect;
    using NHibernate.Driver;
    using NHibernate.Mapping.ByCode;

    using Sakura.Extensions.Data;
    using Sakura.Extensions.Mvc;
    using Sakura.Extensions.Mvc.Web;
    using Sakura.Framework.Fluent;
    using Sakura.Framework.Samples.Contacts.Database.Entities;
    using Sakura.Framework.Samples.Contacts.Database.Schema;
    using Sakura.Framework.Samples.ContactsWeb.Controllers;

    public class Boot
    {
        private static Bootstrapper bootstrapper;

        public static void Shutdown()
        {
            bootstrapper.Shutdown();
        }

        public static void Start()
        {
            bootstrapper = new Setup().Dependencies(
                setup =>
                    {
                        setup.AssemblyOf<AccountController>();
                        setup.AssemblyOf<User>();
                    }).ConfigureMvc(ConfigureRoutes).ConfigureNHibernate(ConfigureNHibernate).Start();
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

            // allow using .NET ISet<T> instead of Iesi ISet<T>
            config.CollectionTypeFactory<Net4CollectionTypeFactory>();
            var mapper = new ModelMapper();

            // entities
            mapper.AddMapping<UserMap>();
            mapper.AddMapping<ContactMap>();

            // compile
            var mapping =
                mapper.CompileMappingFor(
                    typeof(AbstractEntity).Assembly.GetExportedTypes().Where(
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
                "{controller}/{action}/{id}", 
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}