namespace Sakura.Composition.Discovery
{
    using Sakura.Composition.Conventions;

    public class DefaultDependencyDiscoveryTask : DependencyDiscoveryTask
    {
        public DefaultDependencyDiscoveryTask(IDependencyLocator locator)
            : base(locator)
        {
            this.AddConvention(new SingleInstanceConvention());
            this.AddConvention(new TransientConvention());
            this.AddConvention(new UnitOfWorkConvention());
        }
    }
}