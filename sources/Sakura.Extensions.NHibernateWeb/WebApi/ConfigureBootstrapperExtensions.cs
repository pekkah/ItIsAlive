namespace Sakura.Extensions.NHibernateWeb.WebApi
{
    using Autofac;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Tasks;

    public static class ConfigureBootstrapperExtensions
    {
        public static IConfigureBootstrapper EnableWebApiUnitOfWork(this IConfigureBootstrapper configure)
        {
            return
                configure.Dependencies(dependencies => dependencies.AssemblyOf<UnitOfWorkDelegatingHandler>()).Tasks(
                    tasks =>
                    tasks.Add(
                        new ActionTask(context => context.Builder
                            .RegisterType<WebApiUnitOfWorkHandler>().AsSelf())));
        }
    }
}