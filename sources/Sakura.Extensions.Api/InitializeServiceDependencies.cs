namespace Fugu.Extensions.Api
{
    using System.Web.Routing;

    using Autofac;

    using Fugu.Framework.Tasks;

    public class InitializeServiceDependencies : IInitializationTask
    {
        public void Execute(InitializationTaskContext context)
        {
            var builder = context.Builder;

            builder.Register(c => RouteTable.Routes).As<RouteCollection>().SingleInstance();
        }
    }
}