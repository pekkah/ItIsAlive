namespace Sakura.Extensions.Api.Tests
{
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;


    using Machine.Fakes;
    using Machine.Specifications;

    using Sakura.Bootstrapping.Setup;
    using Sakura.Extensions.Api.Tests.Apis;
    using Sakura.Extensions.Api.WebApi;

    public class Given_Api_dependency_and_api_configred_when_started : WithSubject<Setup>
    {
        private static ApiConfiguration configuration1;

        private static ApiConfiguration configuration2;

        private static IContainer container;

        private Establish context = () => Subject.ConfigureWebApi(
            configurationFactory =>
                {
                    configuration1 = configurationFactory();
                    configuration2 = configurationFactory();
                })
                .Dependencies(dependencies => dependencies.Types(typeof(PersonApi)))
                .ExposeContainer(exposed => container = exposed);

        private Because of = () => Subject.Start();

        private It should_create_configuration = () => configuration1.ShouldNotBeNull();

        private It should_create_new_configuration_each_time = () => configuration1.ShouldNotBeTheSameAs(configuration2);

        private It should_register_api = () =>
            {
                var registration =
                    container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(PersonApi))).SingleOrDefault();

                registration.ShouldNotBeNull();
                registration.Lifetime.ShouldBeOfType<CurrentScopeLifetime>();
            };
    }
}