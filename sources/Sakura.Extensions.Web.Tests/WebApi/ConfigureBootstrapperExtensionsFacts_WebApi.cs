namespace Sakura.Extensions.Web.Tests.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using Sakura.Bootstrapping;
    using Sakura.Composition;
    using Sakura.Extensions.Web.Tests.WebApi.Apis;
    using Sakura.Extensions.Web.WebApi;
    using Sakura.Extensions.Web.WebApi.Conventions;

    using Xunit;
    using Xunit.Extensions;

    public class ConfigureBootstrapperExtensionsFacts
    {
        private readonly IConfigureBootstrapper configuration;

        private static ApiConfiguration configuration1;

        private static ApiConfiguration configuration2;

        private static IContainer container;

        public ConfigureBootstrapperExtensionsFacts()
        {
            this.configuration = new Configure().ConfigureWebApi(
                configurationFactory =>
                {
                    configuration1 = configurationFactory();
                    configuration2 = configurationFactory();
                }).Dependencies(dependencies => dependencies.Types(typeof(PersonApi))).ExposeContainer(
                        exposed => container = exposed);

            this.configuration.Start();
        }

        public static IEnumerable<object[]> WebApiConventions
        {
            get
            {
                yield return new object[] { typeof(ServiceContractConvention) };
                yield return new object[] { typeof(DelegatingHandlerConvention) };
                yield return new object[] { typeof(HttpOperationHandlerConvention) };
            }
        }

        [Theory]
        [PropertyData("WebApiConventions")]
        public void should_add_convetions(Type conventionType)
        {
            this.configuration.Conventions(
                conventions => conventions.Should().Contain(c => c.GetType() == conventionType));
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