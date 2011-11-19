namespace Sakura.Framework.Dependencies.Discovery
{
    using System;
    using System.Collections.Generic;

    using Sakura.Framework.Dependencies.Conventions;

    public interface IDependencyLocator
    {
        IEnumerable<Type> GetDependencies(IEnumerable<IRegistrationConvention> policies);
    }
}