namespace Sakura.Framework.Tests.Bootstrapping.BehaviorConfigs
{
    using System.Collections.Generic;

    using Machine.Fakes;

    using Sakura.Framework.Dependencies.Conventions;

    public class DefaultConventions
    {
        private OnEstablish context = fake =>
            {
                var conventions = new IRegistrationConvention[]
                    {
                        new TransientConvention(),
                        new SingleInstanceConvention(),
                        new AsSelfConvention()
                    };

                fake.Configure<IEnumerable<IRegistrationConvention>>(conventions);
            };
    }
}