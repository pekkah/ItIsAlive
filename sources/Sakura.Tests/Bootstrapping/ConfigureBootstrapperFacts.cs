﻿namespace Sakura.Framework.Tests.Bootstrapping
{
    using System;

    using Autofac;
    using Autofac.Builder;

    using FluentAssertions;

    using NSubstitute;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Tasks;
    using Sakura.Composition;
    using Sakura.Framework.Tests.StaticMocks;

    using Xunit;

    public class ConfigureBootstrapperFacts
    {
        private readonly Configure configure;

        public ConfigureBootstrapperFacts()
        {
            this.configure = new Configure();
        }

        [Fact]
        public void should_add_and_execute_task()
        {
            var task = Substitute.For<IInitializationTask>();
            this.configure.Tasks(tasks => tasks.Add(task));

            this.configure.Start();

            task.Received().Execute(Arg.Any<InitializationTaskContext>());
        }

        [Fact]
        public void should_expose_container()
        {
            IContainer container = null;

            this.configure.ExposeContainer(exposed => container = exposed).Start();

            container.Should().NotBeNull();
        }

        [Fact]
        public void should_register_dependencies_from_assembly()
        {
            IContainer container = null;
            this.configure.Dependencies(from => from.AssemblyOf<MockTransientDependency>());
            this.configure.ExposeContainer(exposed => container = exposed).Start();

            var mockTransientDependency = container.ResolveOptional<IMockTransientDependency>();

            mockTransientDependency.Should().NotBeNull();
        }

        [Fact]
        public void should_register_dependencies_from_list_of_types()
        {
            IContainer container = null;
            this.configure.Dependencies(from => from.Types(typeof(MockTransientDependency)));
            this.configure.ExposeContainer(exposed => container = exposed).Start();

            var mockTransientDependency = container.ResolveOptional<IMockTransientDependency>();

            mockTransientDependency.Should().NotBeNull();
        }

        [Fact]
        public void should_use_convention()
        {
            IContainer container = null;

            // setup custom convention
            var convention = this.GetConventionForType(typeof(MockDependency), typeof(IMockDependency));

            // configure
            this.configure.Dependencies(from => from.AssemblyOf<MockTransientDependency>());
            this.configure.Conventions(conventions => conventions.Add(convention));
            this.configure.ExposeContainer(exposed => container = exposed).Start();

            // assert
            var mockDependency = container.ResolveOptional<IMockDependency>();
            mockDependency.Should().NotBeNull();
        }

        private IRegistrationConvention GetConventionForType(Type dependencyType, Type itf)
        {
            var convention = Substitute.For<IRegistrationConvention>();
            convention.IsMatch(Arg.Is<Type>(type => itf.IsAssignableFrom(type))).Returns(true);

            convention.When(c => c.Apply(Arg.Any<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>(), Arg.Any<Type>())).Do(ci =>
                {
                    var dpr =
                        ci.Arg<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>();

                    dpr.As(itf);
                });

            return convention;
        }
    }
}