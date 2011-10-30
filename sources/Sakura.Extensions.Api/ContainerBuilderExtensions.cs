namespace Sakura.Extensions.Api
{
    using Autofac.Builder;

    using Sakura.Extensions.Api.WebApi;

    public static class RegistrationBuilderExtensions
    {
        public static void InstancePerWebApiRequest<T>(
            this IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> builder)
        {
            builder.InstancePerMatchingLifetimeScope(ApiConfiguration.WebApiRequestToken);
        }
    }
}