namespace Sakura.Extensions.Mvc
{
    using System.Web.Routing;

    using Autofac;

    using Sakura.Framework.Tasks;

    public class InitializeHttpDependencies : IInitializationTask
    {
        public void Execute(InitializationTaskContext context)
        {
            var builder = context.Builder;

            builder.Register(c => RouteTable.Routes).As<RouteCollection>().SingleInstance();
        }
    }
}