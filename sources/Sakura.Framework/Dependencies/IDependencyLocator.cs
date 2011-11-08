namespace Sakura.Framework.Dependencies
{
    using System;
    using System.Collections.Generic;

    using Sakura.Framework.Registration;

    public interface IDependencyLocator
    {
        IEnumerable<Type> GetDependencies(IEnumerable<IRegistrationPolicy> policies);
    }
}