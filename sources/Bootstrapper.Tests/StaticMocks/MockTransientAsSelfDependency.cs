namespace Bootstrapper.Framework.Tests.StaticMocks
{
    using Bootstrapper.Composition.Markers;

    public class MockTransientAsSelfDependency : IMockTransientDependency, IAsSelf
    {
    }
}