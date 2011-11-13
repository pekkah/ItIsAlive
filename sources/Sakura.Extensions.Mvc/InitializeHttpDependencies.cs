namespace Sakura.Extensions.Mvc
{
    using System.Web.Routing;

    using Autofac;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Discovery;

    [NotDiscoverable]
    public class InitializeHttpDependencies : IInitializationTask, ISingleInstanceDependency
    {
        public void Execute(InitializationTaskContext context)
        {
            var builder = context.Builder;

            builder.Register(c => RouteTable.Routes).As<RouteCollection>().SingleInstance();
        }
    }
}