namespace Sakura.Extensions.Api
{
    using System;

    using Autofac;

    using Sakura.Extensions.Api.WebApi;
    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Tasks;

    public class InitializeWebApi : IInitializationTask, ITransientDependency
    {
        private readonly Action<IWebApiRouter, ApiConfiguration> configure;

        public InitializeWebApi(Action<IWebApiRouter, ApiConfiguration> configure)
        {
            this.configure = configure;
        }

        public void Execute(InitializationTaskContext context)
        {
            var builder = context.Builder;

            builder.RegisterInstance(this.configure).AsSelf();
        }
    }
}