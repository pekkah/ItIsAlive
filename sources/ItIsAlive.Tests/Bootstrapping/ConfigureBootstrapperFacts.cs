namespace ItIsAlive.Framework.Tests.Bootstrapping
{
    using System;
    using Autofac;
    using Autofac.Builder;
    using FluentAssertions;
    using ItIsAlive.Composition;
    using NSubstitute;
    using StaticMocks;
    using Tasks;
    using Xunit;

    public class ConfigureBootstrapperFacts
    {
        private readonly IIt _itIs;

        public ConfigureBootstrapperFacts()
        {
            _itIs = It.Is;
        }

        [Fact]
        public void should_add_and_execute_task()
        {
            var task = Substitute.For<IInitializationTask>();
            _itIs.Sequence(tasks => tasks.Add(task));

            _itIs.Alive();

            task.Received().Execute(Arg.Any<InitializationTaskContext>());
        }

        [Fact]
        public void should_expose_container()
        {
            IContainer container = null;

            _itIs.ExposeContainer(exposed => container = exposed).Alive();

            container.Should().NotBeNull();
        }

        [Fact]
        public void should_register_dependencies_from_assembly()
        {
            IContainer container = null;
            _itIs.Composed(from => from.AssemblyOf<MockTransientDependency>());
            _itIs.ExposeContainer(exposed => container = exposed).Alive();

            var mockTransientDependency = container.ResolveOptional<IMockTransientDependency>();

            mockTransientDependency.Should().NotBeNull();
        }

        [Fact]
        public void should_register_dependencies_from_list_of_types()
        {
            IContainer container = null;
            _itIs.Composed(from => from.Types(typeof (MockTransientDependency)));
            _itIs.ExposeContainer(exposed => container = exposed).Alive();

            var mockTransientDependency = container.ResolveOptional<IMockTransientDependency>();

            mockTransientDependency.Should().NotBeNull();
        }

        [Fact]
        public void should_use_convention()
        {
            IContainer container = null;

            // setup custom convention
            var convention = GetConventionForType(typeof (MockDependency), typeof (IMockDependency));

            // ItIs
            _itIs.Composed(from => from.AssemblyOf<MockTransientDependency>());
            _itIs.Using(conventions => conventions.Add(convention));
            _itIs.ExposeContainer(exposed => container = exposed).Alive();

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