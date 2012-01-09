namespace Sakura.Bootstrapping
{
    using System;
    using System.Collections.Generic;

    using Autofac;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Composition;

    public interface IConfigureBootstrapper
    {
        IConfigureBootstrapper Conventions(Action<IList<IRegistrationConvention>> conventions);

        IConfigureBootstrapper ExposeContainer(Action<IContainer> exposeTo);

        Bootstrapper Start();

        IConfigureBootstrapper Tasks(Action<IList<IInitializationTask>> modifyTasks);

        IConfigureBootstrapper Dependencies(Action<IConfigureDependencies> dependencies);
    }
}