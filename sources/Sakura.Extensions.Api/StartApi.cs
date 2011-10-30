namespace Fugu.Extensions.Api
{
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;

    using Autofac;

    using Fugu.Extensions.Api.Apis;
    using Fugu.Extensions.Api.WebApi;
    using Fugu.Framework.Tasks;

    public class StartApi : IStartupTask
    {
        private readonly ILifetimeScope lifetimeScope;

        private readonly IRouting routes;

        public StartApi(ILifetimeScope lifetimeScope, IRouting routes)
        {
            this.lifetimeScope = lifetimeScope;
            this.routes = routes;
        }

        public void Execute()
        {
            var config = new ApiConfiguration(this.lifetimeScope) { EnableTestClient = true };

            var odataFormatter = new ODataMediaTypeFormatter();
            odataFormatter.SupportedMediaTypes.Clear();
            odataFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/atom+json"));

            config.Formatters.Add(odataFormatter);

            this.routes.SetDefaultHttpConfiguration(config);
            this.routes.MapServiceRoute<ProjectsApi>("api/projects");
        }
    }
}