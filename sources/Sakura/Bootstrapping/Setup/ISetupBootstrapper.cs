namespace Sakura.Bootstrapping.Setup
{
    using System;
    using System.Collections.Generic;

    using Autofac;

    using Sakura.Bootstrapping;
    using Sakura.Bootstrapping.Tasks;
    using Sakura.Framework.Dependencies.Policies;

    public interface ISetupBootstrapper
    {
        ISetupBootstrapper Dependencies(Action<ISetupDependencies> setupDependencies);

        ISetupBootstrapper ExposeContainer(Action<IContainer> exposeTo);

        ISetupBootstrapper RegistrationPolicies(Action<ISet<IRegistrationPolicy>> policies);

        Bootstrapper Start();

        ISetupBootstrapper Tasks(Action<InitializationTaskManager> tasks);
    }
}