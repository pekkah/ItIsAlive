namespace Fugu.Extensions.Api.WebApi
{
    using Fugu.Framework.Dependencies;

    using Microsoft.ApplicationServer.Http;

    public interface IRouting : ISingleInstanceDependency
    {
        void MapServiceRoute<TService>(string routePrefix, HttpConfiguration configuration =null, object constraints = null, bool useMethodPrefixForHttpMethod = true);

        void SetDefaultHttpConfiguration(ApiConfiguration config);
    }
}