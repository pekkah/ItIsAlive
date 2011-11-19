namespace Sakura.Extensions.NHibernateWebApi
{
    using System.Net.Http;

    using Autofac;

    using Microsoft.ApplicationServer.Http.Dispatcher;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Extensions.NHibernate;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Discovery;

    [NotDiscoverable]
    public class RegisterWorkContextHandler : IInitializationTask, ISingleInstanceDependency
    {
        public void Execute(InitializationTaskContext context)
        {
            context.Builder.RegisterType<WorkContextOperationHandler>()
                .As<HttpOperationHandler<HttpRequestMessage, IWorkContext>>()
                .InstancePerLifetimeScope();
        }
    }
}