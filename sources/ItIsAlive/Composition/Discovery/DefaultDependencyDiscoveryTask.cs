namespace ItIsAlive.Composition.Discovery
{
    using Conventions;

    public class DefaultDependencyDiscoveryTask : DependencyDiscoveryTask
    {
        public DefaultDependencyDiscoveryTask(IDependencyLocator locator)
            : base(locator)
        {
            AddConvention(new SingleInstanceConvention());
            AddConvention(new TransientConvention());
            AddConvention(new UnitOfWorkConvention());
        }
    }
}