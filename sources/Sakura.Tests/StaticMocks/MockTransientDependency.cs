namespace Sakura.Framework.Tests.StaticMocks
{
    using Sakura.Composition.Markers;

    public interface IMockTransientDependency : ITransientDependency
    {
        
    }

    public class MockTransientDependency : IMockTransientDependency
    {
    }
}