namespace ItIsAlive.Framework.Tests.StaticMocks
{
    using ItIsAlive.Composition.Markers;

    public interface IMockSingleInstanceDependency : ISingleInstanceDependency
    {
    }

    public class MockSingleInstanceDependency : IMockSingleInstanceDependency
    {
    }
}