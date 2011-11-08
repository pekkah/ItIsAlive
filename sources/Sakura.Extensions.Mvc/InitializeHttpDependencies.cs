namespace Sakura.Extensions.Mvc
{
    using System.Web.Routing;

    using Autofac;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Discovery;
    using Sakura.Framework.Tasks;
    using Sakura.Framework.Tasks.Types;

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