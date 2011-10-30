namespace Sakura.Framework.Dependencies
{
    using System;
    using System.Collections.Generic;

    public interface IDependencyLocator
    {
        IEnumerable<Type> GetDependencies();
    }
}