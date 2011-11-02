namespace Sakura.Extensions.Api.WebApi
{
    using System.Web.Routing;

    using Microsoft.ApplicationServer.Http;

    public class WebApiRouter : IWebApiRouter
    {
        private readonly RouteCollection routes;

        public WebApiRouter(RouteCollection routes)
        {
            this.routes = routes;
        }

        public void MapServiceRoute<TService>(
            string routePrefix, 
            HttpConfiguration configuration = null, 
            object constraints = null, 
            bool useMethodPrefixForHttpMethod = true)
        {
            this.routes.MapServiceRoute<TService>(routePrefix, configuration, constraints, useMethodPrefixForHttpMethod);
        }

        public void SetDefaultHttpConfiguration(ApiConfiguration configuration)
        {
            this.routes.SetDefaultHttpConfiguration(configuration);
        }
    }
}