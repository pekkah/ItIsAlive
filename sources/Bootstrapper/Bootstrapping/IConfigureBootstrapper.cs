namespace Bootstrapper.Bootstrapping
{
    using System;
    using System.Collections.Generic;

    using Autofac;

    using Composition;

    using Tasks;

    public interface IConfigureBootstrapper
    {
        IConfigureBootstrapper Conventions(Action<ICollection<IRegistrationConvention>> configureConventions);

        IConfigureBootstrapper Dependencies(Action<IConfigureDependencies> from);

        IConfigureBootstrapper ExposeContainer(Action<IContainer> exposedContainer);

        Bootstrapper Start();

        IConfigureBootstrapper Tasks(Action<ICollection<IInitializationTask>> tasks);
    }
}