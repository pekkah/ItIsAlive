namespace Sakura.Extensions.WebApi.Tests
{
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using Sakura.Bootstrapping;
    using Sakura.Extensions.WebApi.Tests.Apis;
    using Sakura.Extensions.WebApi.WebApi;

    using Xunit;

    public class ConfigureBootstrapperExtensionsFacts
    {
        private static ApiConfiguration configuration1;

        private static ApiConfiguration configuration2;

        private static IContainer container;

        public ConfigureBootstrapperExtensionsFacts()
        {
            new Configure().ConfigureWebApi(
                configurationFactory =>
                    {
                        configuration1 = configurationFactory();
                        configuration2 = configurationFactory();
                    }).Dependencies(dependencies => dependencies.Types(typeof(PersonApi))).ExposeContainer(
                        exposed => container = exposed).Start();
        }

        [Fact]
        public void should_create_configuration()
        {
            configuration1.Should().NotBeNull();
        }

        [Fact]
        public void should_create_new_configuration_each_time()
        {
            configuration1.Should().NotBeSameAs(configuration2);
        }

        [Fact]
        public void should_register_api()
        {
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(PersonApi))).SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }
    }
}