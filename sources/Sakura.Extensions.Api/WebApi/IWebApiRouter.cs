namespace Sakura.Extensions.Api.WebApi
{
    using Microsoft.ApplicationServer.Http;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Dependencies.DefaultTypes;

    public interface IWebApiRouter : ISingleInstanceDependency
    {
        void MapServiceRoute<TService>(
            string routePrefix, 
            HttpConfiguration configuration = null, 
            object constraints = null, 
            bool useMethodPrefixForHttpMethod = true);

        void SetDefaultHttpConfiguration(ApiConfiguration config);
    }
}