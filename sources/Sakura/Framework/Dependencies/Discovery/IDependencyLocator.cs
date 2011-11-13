namespace Sakura.Framework.Dependencies.Discovery
{
    using System;
    using System.Collections.Generic;

    using Sakura.Framework.Dependencies.Policies;

    public interface IDependencyLocator
    {
        IEnumerable<Type> GetDependencies(IEnumerable<IRegistrationPolicy> policies);
    }
}