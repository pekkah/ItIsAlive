namespace Bootstrapper.Framework.Tests.Bootstrapping
{
    using System;

    using Autofac;
    using Autofac.Builder;

    using Bootstrapper.Bootstrapping;
    using Bootstrapper.Bootstrapping.Tasks;
    using Bootstrapper.Composition;

    using FluentAssertions;

    using NSubstitute;

    using StaticMocks;

    using Xunit;

    public class ConfigureBootstrapperFacts
    {
        private readonly Configure configure;

        public ConfigureBootstrapperFacts()
        {
            configure = new Configure();
        }

        [Fact]
        public void should_add_and_execute_task()
        {
            var task = Substitute.For<IInitializationTask>();
            configure.Tasks(tasks => tasks.Add(task));

            configure.Start();

            task.Received().Execute(Arg.Any<InitializationTaskContext>());
        }

        [Fact]
        public void should_expose_container()
        {
            IContainer container = null;

            configure.ExposeContainer(exposed => container = exposed).Start();

            container.Should().NotBeNull();
        }

        [Fact]
        public void should_register_dependencies_from_assembly()
        {
            IContainer container = null;
            configure.Dependencies(from => from.AssemblyOf<MockTransientDependency>());
            configure.ExposeContainer(exposed => container = exposed).Start();

            var mockTransientDependency = container.ResolveOptional<IMockTransientDependency>();

            mockTransientDependency.Should().NotBeNull();
        }

        [Fact]
        public void should_register_dependencies_from_list_of_types()
        {
            IContainer container = null;
            configure.Dependencies(from => from.Types(typeof(MockTransientDependency)));
            configure.ExposeContainer(exposed => container = exposed).Start();

            var mockTransientDependency = container.ResolveOptional<IMockTransientDependency>();

            mockTransientDependency.Should().NotBeNull();
        }

        [Fact]
        public void should_use_convention()
        {
            IContainer container = null;

            // setup custom convention
            var convention = GetConventionForType(typeof(MockDependency), typeof(IMockDependency));

            // configure
            configure.Dependencies(from => from.AssemblyOf<MockTransientDependency>());
            configure.Conventions(conventions => conventions.Add(convention));
            configure.ExposeContainer(exposed => container = exposed).Start();

            // assert
            var mockDependency = container.ResolveOptional<IMockDependency>();
            mockDependency.Should().NotBeNull();
        }

        private IRegistrationConvention GetConventionForType(Type dependencyType, Type itf)
        {
            var convention = Substitute.For<IRegistrationConvention>();
            convention.IsMatch(Arg.Is<Type>(type => itf.IsAssignableFrom(type))).Returns(true);

            convention.When(
                c =>
                c.Apply(
                    Arg.Any<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>(),
                    Arg.Any<Type>())).Do(
                        ci =>
                            {
                                var dpr =
                                    ci
                                        .Arg
                                        <
                                            IRegistrationBuilder
                                                <object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>();

                                dpr.As(itf);
                            });

            return convention;
        }
    }
}