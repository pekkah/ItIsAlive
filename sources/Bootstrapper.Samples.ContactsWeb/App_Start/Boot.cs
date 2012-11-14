namespace Bootstrapper.Samples.ContactsWeb.App_Start
{
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Autofac;
    using Autofac.Integration.Mvc;
    using Autofac.Integration.WebApi;

    using Bootstrapping;
    using Bootstrapping.Tasks;

    using Controllers;

    using Database.Entities;
    using Database.Schema;

    using Extensions.NHibernate;

    using Filters;

    using NHibernate.Cfg;
    using NHibernate.Dialect;
    using NHibernate.Driver;
    using NHibernate.Mapping.ByCode;

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
                //.ConfigureMvc(ConfigureRoutes)
                                          .ConfigureNHibernate(ConfigureNHibernate)
                //.ConfigureWebApi(ConfigureApis)
                //.EnableMvcUnitOfWork()
                //.EnableWebApiUnitOfWork()
                                          .WarmupNHibernate().Tasks(
                                              tasks =>
                                                  {
                                                      tasks.Add(new ActionTask(ConfigureUnitOfWork));
                                                      tasks.Add(new ActionTask(ConfigureApis));
                                                      tasks.Add(new ActionTask(ConfigureRoutes));
                                                  })
                                          .ExposeContainer(ConfigureResolvers)
                                          .Start();
        }

        private static void ConfigureResolvers(IContainer container)
        {
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void ConfigureUnitOfWork(InitializationTaskContext context)
        {
        }

        private static void ConfigureApis(InitializationTaskContext initializationTaskContext)
        {
            var configuration = GlobalConfiguration.Configuration;
            configuration.MessageHandlers.Insert(0, new WebApiUnitOfWorkHandler());

            configuration.Routes.MapHttpRoute("contacts", "api/contacts", new { controller = "Contacts" });

            initializationTaskContext.Builder.RegisterApiControllers(typeof(ContactsController).Assembly);
            initializationTaskContext.Builder.RegisterWebApiFilterProvider(configuration);
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
                    typeof(AbstractEntity).Assembly.GetExportedTypes()
                                          .Where(type => typeof(AbstractEntity).IsAssignableFrom(type)));

            // use mappings
            config.AddMapping(mapping);

            return config;
        }

        private static void ConfigureRoutes(InitializationTaskContext initializationTaskContext)
        {
            var routes = RouteTable.Routes;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            initializationTaskContext.Builder.RegisterControllers(typeof(ContactsController).Assembly);
            initializationTaskContext.Builder.RegisterFilterProvider();
        }
    }
}