namespace ItIsAlive.Framework.Tests.Composition
{
    using System;

    using ItIsAlive.Composition.Conventions;
    using ItIsAlive.Composition.Discovery;

    using FluentAssertions;

    using NSubstitute;

    using Xunit;

    public class DefaultDependencyDiscoveryTaskFacts : IDisposable
    {
        private DefaultDependencyDiscoveryTask discoveryTask;

        public DefaultDependencyDiscoveryTaskFacts()
        {
            var locator = Substitute.For<IDependencyLocator>();
            discoveryTask = new DefaultDependencyDiscoveryTask(locator);
        }

        public void Dispose()
        {
            discoveryTask = null;
        }

        [Fact]
        public void should_register_singleInstance_convention()
        {
            discoveryTask.Conventions.Should().Contain(p => p.GetType() == typeof(SingleInstanceConvention));
        }

        [Fact]
        public void should_register_transient_convention()
        {
            discoveryTask.Conventions.Should().Contain(p => p.GetType() == typeof(TransientConvention));
        }

        [Fact]
        public void should_register_unitOfWork_convention()
        {
            discoveryTask.Conventions.Should().Contain(p => p.GetType() == typeof(UnitOfWorkConvention));
        }
    }
}