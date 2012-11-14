namespace ItIsAlive.Framework.Tests.StaticMocks
{
    using ItIsAlive.Composition.Markers;

    public interface IMockTransientDependency : ITransientDependency
    {
    }

    public class MockTransientDependency : IMockTransientDependency
    {
    }
}