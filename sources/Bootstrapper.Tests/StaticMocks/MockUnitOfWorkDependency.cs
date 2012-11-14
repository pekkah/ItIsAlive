namespace Bootstrapper.Framework.Tests.StaticMocks
{
    using Bootstrapper.Composition.Markers;

    public interface IMockUnitOfWorkDependency : IUnitOfWorkDependency
    {
    }

    public class MockUnitOfWorkDependency : IMockUnitOfWorkDependency
    {
    }
}