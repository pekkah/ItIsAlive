using Sakura.Samples.ContactsWeb.App_Start;

using WebActivator;

[assembly: PostApplicationStartMethod(typeof(Boot), "Start")]
[assembly: ApplicationShutdownMethod(typeof(Boot), "Shutdown")]

namespace Sakura.Samples.ContactsWeb.App_Start
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Autofac.Integration.Mvc;

    using NHibernate.Cfg;
    using NHibernate.Dialect;
    using NHibernate.Driver;
    using NHibernate.Mapping.ByCode;

    using Sakura.Bootstrapping;
    using Sakura.Composition;
    using Sakura.Extensions.WebApi;
    using Sakura.Extensions.WebApi.WebApi;
    using Sakura.Extensions.Mvc;
    using Sakura.Extensions.NHibernate;
    using Sakura.Extensions.NHibernateMvc;
    using Sakura.Extensions.NHibernateWebApi;
    using Sakura.Samples.Contacts.Database.Entities;
    using Sakura.Samples.Contacts.Database.Schema;
    using Sakura.Samples.ContactsWeb.Apis;
    using Sakura.Samples.ContactsWeb.Controllers;

    public class Boot
    {
        private static Bootstrapper bootstrapper;

        public static void Shutdown()
        {
            bootstrapper.Shutdown();
        }

        public static void Start()
        {
            bootstrapper = new Configure().Dependencies(
                setup =>
                    {
                        setup.AssemblyOf<AccountController>();
                        setup.AssemblyOf<User>();
                    })
                    .ConfigureMvc(ConfigureRoutes)
                    .ConfigureNHibernate(ConfigureNHibernate)
                    .ConfigureWebApi(ConfigureApis)
                    .EnableMvcUnitOfWork()
                    .EnableWebApiUnitOfWork()
                    .WarmupNHibernate()
                    .Start();
        }

        private static void ConfigureApis(Func<ApiConfiguration> configurationFactory)
        {
            var routes = RouteTable.Routes;

            var configuration = configurationFactory();

            routes.SetDefaultHttpConfiguration(configuration);
            routes.MapServiceRoute<ContactsApi>("api/contacts");
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

        private static void ConfigureRoutes()
        {
            var routes = RouteTable.Routes;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", 
                "{controller}/{action}/{id}", 
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}