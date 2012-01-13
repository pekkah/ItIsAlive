namespace Sakura.Extensions.Web.WebApi
{
    using System;

    using Autofac;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Composition.Discovery;

    [Hidden]
    public class InitializeWebApi : IInitializationTask
    {
        private readonly Action<Func<ApiConfiguration>> configurationFactory;

        public InitializeWebApi(Action<Func<ApiConfiguration>> configurationFactory)
        {
            this.configurationFactory = configurationFactory;
        }

        public void Execute(InitializationTaskContext context)
        {
            context.Builder.Register(
                componentContext =>
                new StartWebApi(componentContext.Resolve<ILifetimeScope>(), this.configurationFactory)).
                AsImplementedInterfaces();
        }
    }
}