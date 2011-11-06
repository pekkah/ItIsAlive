namespace Sakura.Extensions.Mvc
{
    using System.Web.Routing;

    using Autofac;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Tasks;

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