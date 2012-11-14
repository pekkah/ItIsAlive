namespace ItIsAlive.Composition.Discovery
{
    using System;
    using System.Collections.Generic;

    public interface IDependencyLocator
    {
        IEnumerable<Type> GetDependencies(IEnumerable<IRegistrationConvention> conventions);
    }
}