﻿namespace Sakura.Framework.Tests.Bootstrapping
{
    using System.Linq;
    using System.Reflection;

    using Autofac;
    using Autofac.Core;

    using FluentAssertions;

    using NUnit.Framework;

    using Sakura.Bootstrapping.Configuration;
    using Sakura.Framework.Tests.Bootstrapping.DependencyRegistration.Mocks;

    [TestFixture]
    public class When_booting
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void should_expose_container()
        {
            IContainer container = null;

            var currentAssembly = Assembly.GetExecutingAssembly();

            var bootstrapper =
                new Setup().Dependencies(setup => setup.Assembly(currentAssembly)).ExposeContainer(
                    exposedContainer => container = exposedContainer).Start();

            container.Should().NotBeNull();
        }

        [Test]
        public void should_register_dependencies_from_assemblies()
        {
            IContainer container = null;

            var currentAssembly = Assembly.GetExecutingAssembly();

            var bootstrapper =
                new Setup().Dependencies(setup => setup.Assembly(currentAssembly)).ExposeContainer(
                    exposedContainer => container = exposedContainer).Start();

            var registration =
                container.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IMockTransientDependency))).
                    SingleOrDefault();

            registration.Should().NotBeNull();
        }
    }
}