namespace ItIsAlive.Framework.Tests.StaticMocks
{
    using ItIsAlive.Composition.Markers;

    public class MockTransientAsSelfDependency : IMockTransientDependency, IAsSelf
    {
    }
}