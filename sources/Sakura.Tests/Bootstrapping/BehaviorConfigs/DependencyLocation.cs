namespace Sakura.Framework.Tests.Bootstrapping.BehaviorConfigs
{
    using System.Collections.Generic;

    using Machine.Fakes;

    using NSubstitute;

    using Sakura.Bootstrapping;
    using Sakura.Framework.Dependencies.Conventions;
    using Sakura.Framework.Dependencies.Discovery;
    using Sakura.Framework.Tests.Bootstrapping.Mocks;

    public class DependencyLocation
    {
        private OnEstablish context = fake =>
            {
                var locator = fake.An<IDependencyLocator>();

                locator.GetDependencies(Arg.Any<IEnumerable<IRegistrationConvention>>()).Returns(new[] { typeof(MockSingleInstanceDependency), typeof(MockTransientDependency) });

                fake.Configure(locator);
            };
    }
}