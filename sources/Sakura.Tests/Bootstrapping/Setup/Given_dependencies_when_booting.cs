namespace Sakura.Framework.Tests.Bootstrapping.Setup
{
    using System.Linq;
    using System.Reflection;

    using Autofac;
    using Autofac.Core;

    using Machine.Fakes;
    using Machine.Specifications;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Setup;
    using Sakura.Framework.Tests.Bootstrapping.Mocks;

    public class Given_dependencies_when_booting : WithSubject<Setup>
    {
        private static Bootstrapper bootstrapper;

        private static IContainer container;

        private static Assembly currentAssembly;

        private Establish context = () => { currentAssembly = Assembly.GetExecutingAssembly(); };

        private Because of =
            () =>
                {
                    bootstrapper =
                        Subject.Dependencies(setup => setup.Assembly(currentAssembly)).ExposeContainer(
                            exposedContainer => container = exposedContainer).Start();
                };

        private It should_expose_container = () => container.ShouldNotBeNull();

        private It should_register_dependencies_from_assemblies = () =>
            {
                var registration =
                    container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockTransientDependency))).
                        SingleOrDefault();

                registration.ShouldNotBeNull();
            };
    }
}