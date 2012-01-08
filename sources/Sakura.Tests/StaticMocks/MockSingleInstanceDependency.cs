namespace Sakura.Framework.Tests.StaticMocks
{
    using Sakura.Composition.Markers;

    public interface IMockSingleInstanceDependency : ISingleInstanceDependency
    {
    }

    public class MockSingleInstanceDependency : IMockSingleInstanceDependency
    {
    }
}