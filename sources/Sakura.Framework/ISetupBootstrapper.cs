namespace Sakura.Framework
{
    using System;

    using Autofac;

    using Sakura.Framework.Registration;
    using Sakura.Framework.Tasks;

    public interface ISetupBootstrapper
    {
        ISetupBootstrapper Dependencies(Action<ISetupDependencies> setupDependencies);

        Bootstrapper Start();

        ISetupBootstrapper Task(IInitializationTask task);

        ISetupBootstrapper TryTask(IInitializationTask http);

        ISetupBootstrapper ExposeContainer(Action<IContainer> exposeTo);

        ISetupBootstrapper AddPolicy(IRegistrationPolicy policy);
    }
}