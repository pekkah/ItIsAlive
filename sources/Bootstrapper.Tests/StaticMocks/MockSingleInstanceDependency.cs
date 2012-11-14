namespace Bootstrapper.Framework.Tests.StaticMocks
{
    using Bootstrapper.Composition.Markers;

    public interface IMockSingleInstanceDependency : ISingleInstanceDependency
    {
    }

    public class MockSingleInstanceDependency : IMockSingleInstanceDependency
    {
    }
}