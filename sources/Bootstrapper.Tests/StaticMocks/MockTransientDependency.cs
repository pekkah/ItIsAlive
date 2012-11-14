namespace Bootstrapper.Framework.Tests.StaticMocks
{
    using Bootstrapper.Composition.Markers;

    public interface IMockTransientDependency : ITransientDependency
    {
    }

    public class MockTransientDependency : IMockTransientDependency
    {
    }
}