namespace Sakura.Extensions.Mvc.Tests
{
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Lifetime;

    using Machine.Fakes;
    using Machine.Specifications;

    using Sakura.Bootstrapping;
    using Sakura.Composition;
    using Sakura.Extensions.Mvc.Conventions;
    using Sakura.Extensions.Mvc.Tests.Controllers;
    using Sakura.Extensions.Mvc.Tests.Filters;

    public class Given_dependencies_and_mvc_configured_when_started : WithSubject<Configure>
    {
        private static IContainer container;

        private Establish context = () => Subject.ConfigureMvc(() =>
            {
               Trace.TraceInformation("Configuring"); 
            })
            .Dependencies(dependencies => dependencies.AssemblyOf<PersonController>())
            .ExposeContainer(exposed => container = exposed);

        private Because of = () => Subject.Start();

        private It should_register_controllers = () =>
            {
                var registration =
                    container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(PersonController))).SingleOrDefault();

                registration.ShouldNotBeNull();
                registration.Lifetime.ShouldBeOfType<CurrentScopeLifetime>();
            };

        private It should_register_global_filters = () =>
            {
                var registration =
                    container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IGlobalFilter))).SingleOrDefault();

                registration.ShouldNotBeNull();
                registration.Lifetime.ShouldBeOfType<CurrentScopeLifetime>();
            };

        private It should_register_model_binders= () =>
        {
            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IModelBinder))).SingleOrDefault();

            registration.ShouldNotBeNull();
            registration.Lifetime.ShouldBeOfType<CurrentScopeLifetime>();
        };
    }
}