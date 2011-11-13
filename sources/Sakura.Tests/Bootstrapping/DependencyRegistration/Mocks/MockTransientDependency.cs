namespace Sakura.Framework.Tests.Bootstrapping.DependencyRegistration.Mocks
{
    using Sakura.Framework.Dependencies.DefaultTypes;

    public interface IMockTransientDependency : ITransientDependency
    {
    }

    public class MockTransientDependency : IMockTransientDependency
    {
    }
}