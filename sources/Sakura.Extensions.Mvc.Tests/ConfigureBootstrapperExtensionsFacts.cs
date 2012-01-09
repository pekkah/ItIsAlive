namespace Sakura.Extensions.Mvc.Tests
{
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using FluentAssertions;

    using Sakura.Bootstrapping;
    using Sakura.Extensions.Mvc.Tests.Controllers;

    using Xunit;

    public class ConfigureBootstrapperExtensionsFacts
    {
        private static IContainer container;

        private Bootstrapper bootstrapper;

        public ConfigureBootstrapperExtensionsFacts()
        {
            this.bootstrapper =
                new Configure().ConfigureMvc(() => Trace.TraceInformation("Configuring")).Dependencies(
                    from => from.AssemblyOf<PersonController>()).ExposeContainer(exposed => container = exposed).Start();
        }

        [Fact]
        public void should_register_controllers()
        {
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(PersonController))).SingleOrDefault
                    ();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }

        [Fact]
        public void should_register_global_filters()
        {
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IGlobalFilter))).SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }

        [Fact]
        public void should_register_model_binders()
        {
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IModelBinder))).SingleOrDefault();

            registration.Should().NotBeNull();
            registration.Lifetime.Should().BeOfType<CurrentScopeLifetime>();
        }
    }
}