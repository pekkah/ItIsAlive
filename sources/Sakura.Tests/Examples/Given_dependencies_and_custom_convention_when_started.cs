namespace Sakura.Framework.Tests.Examples
{
    using System;

    using Autofac;

    using Machine.Fakes;
    using Machine.Specifications;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Setup;
    using Sakura.Framework.Dependencies.Conventions;

    public interface IMyService
    {
    }

    public class MyService : IMyService
    {
    }

    public class NameEndsWithServiceConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            builder.RegisterType(dependencyType).AsImplementedInterfaces().InstancePerDependency();
        }

        public bool IsMatch(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                return false;
            }

            return type.FullName.EndsWith("Service");
        }
    }

    public class Given_dependencies_and_custom_convention_when_started : WithSubject<Setup>
    {
        private static IContainer container;

        private Establish context = () => Subject
            .Dependencies(from => from.AssemblyOf<IMyService>())
            .Conventions(conventions => conventions.Add(new NameEndsWithServiceConvention()));

        private Because of = () => Subject.ExposeContainer(exposed => container = exposed).Start();

        private It should_register_dependencies_by_implemented_interfaces = () =>
            {
                var myService = container.Resolve<IMyService>();

                myService.ShouldNotBeNull();
                myService.ShouldBeOfType<MyService>();
            };
    }
}