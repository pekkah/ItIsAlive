namespace Sakura.Framework.Fluent
{
    using System;

    using Autofac;

    using Sakura.Framework.Dependencies.Policies;
    using Sakura.Framework.Tasks.Types;

    public interface ISetupBootstrapper
    {
        ISetupBootstrapper AddPolicy(IRegistrationPolicy policy);

        ISetupBootstrapper Dependencies(Action<ISetupDependencies> setupDependencies);

        ISetupBootstrapper ExposeContainer(Action<IContainer> exposeTo);

        Bootstrapper Start();

        ISetupBootstrapper Task(IInitializationTask task);

        ISetupBootstrapper TryTask(IInitializationTask http);
    }
}