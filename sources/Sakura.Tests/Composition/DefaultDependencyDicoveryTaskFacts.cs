namespace Sakura.Framework.Tests.Composition
{
    using System;

    using FluentAssertions;

    using NSubstitute;

    using Sakura.Composition.Conventions;
    using Sakura.Composition.Discovery;

    using Xunit;

    public class DefaultDependencyDiscoveryTaskFacts : IDisposable
    {
        private DefaultDependencyDiscoveryTask discoveryTask;

        public DefaultDependencyDiscoveryTaskFacts()
        {
            var locator = Substitute.For<IDependencyLocator>();
            this.discoveryTask = new DefaultDependencyDiscoveryTask(locator);
        }

        public void Dispose()
        {
            this.discoveryTask = null;
        }

        [Fact]
        public void should_register_singleInstance_convention()
        {
            this.discoveryTask.Conventions.Should().Contain(p => p.GetType() == typeof(SingleInstanceConvention));
        }

        [Fact]
        public void should_register_transient_convention()
        {
            this.discoveryTask.Conventions.Should().Contain(p => p.GetType() == typeof(TransientConvention));
        }

        [Fact]
        public void should_register_unitOfWork_convention()
        {
            this.discoveryTask.Conventions.Should().Contain(p => p.GetType() == typeof(UnitOfWorkConvention));
        }
    }
}