namespace Sakura.Framework.Tests.Bootstrapping.DependencyRegistration.Mocks
{
    using Sakura.Framework.Dependencies.DefaultTypes;

    public interface IMockSingleInstanceDependency : ISingleInstanceDependency
    {
    }

    public class MockSingleInstanceDependency : IMockSingleInstanceDependency
    {
    }
}