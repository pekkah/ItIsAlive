namespace ItIsAlive
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using Composition;
    using Tasks;

    public interface IIt
    {
        IIt Using(Action<ICollection<IRegistrationConvention>> configureConventions);

        IIt Composed(Action<IConfigureDependencies> from);

        IIt ExposeContainer(Action<IContainer> exposedContainer);

        TheThing Alive();

        IIt Sequence(Action<ICollection<IInitializationTask>> tasks);
    }
}