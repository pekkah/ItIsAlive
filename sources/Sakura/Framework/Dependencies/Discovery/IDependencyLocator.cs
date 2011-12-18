namespace Sakura.Framework.Dependencies.Discovery
{
    using System;
    using System.Collections.Generic;

    using Sakura.Bootstrapping;

    public interface IDependencyLocator
    {
        IEnumerable<Type> GetDependencies(IEnumerable<IRegistrationConvention> conventions);
    }
}