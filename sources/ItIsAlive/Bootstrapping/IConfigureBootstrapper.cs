namespace ItIsAlive.Bootstrapping
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

        Bootstrapper Alive();

        IConfigureBootstrapper Sequence(Action<ICollection<IInitializationTask>> tasks);
    }
}