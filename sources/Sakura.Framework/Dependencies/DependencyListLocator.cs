namespace Sakura.Framework.Dependencies
{
    using System;
    using System.Collections.Generic;

    public class DependencyListLocator : IDependencyLocator
    {
        private readonly Type[] dependencyTypes;

        public DependencyListLocator(params Type[] dependencyTypes)
        {
            if (dependencyTypes == null)
            {
                throw new ArgumentNullException("dependencyTypes");
            }

            this.dependencyTypes = dependencyTypes;
        }

        public IEnumerable<Type> GetDependencies()
        {
            return DependencyLocator.FilterDependencyTypes(this.dependencyTypes);
        }
    }
}