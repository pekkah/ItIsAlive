namespace Sakura.Framework.Fluent
{
    using System;
    using System.Collections.Generic;

    using Autofac;

    using Sakura.Framework.Dependencies.Policies;
    using Sakura.Framework.Tasks;

    public interface ISetupBootstrapper
    {
        ISetupBootstrapper Dependencies(Action<ISetupDependencies> setupDependencies);

        ISetupBootstrapper ExposeContainer(Action<IContainer> exposeTo);

        ISetupBootstrapper RegistrationPolicies(Action<ISet<IRegistrationPolicy>> policies);

        Bootstrapper Start();

        ISetupBootstrapper Tasks(Action<InitializationTaskManager> tasks);
    }
}