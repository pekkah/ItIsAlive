namespace Sakura.Framework.Dependencies
{
    using System;
    using System.Collections.Generic;

    public class TypeLocator : IDependencyLocator
    {
        private readonly Type[] dependencyTypes;

        public TypeLocator(params Type[] dependencyTypes)
        {
            if (dependencyTypes == null)
            {
                throw new ArgumentNullException("dependencyTypes");
            }

            this.dependencyTypes = dependencyTypes;
        }

        public IEnumerable<Type> GetDependencies()
        {
            return AssemblyLocator.FilterDependencyTypes(this.dependencyTypes);
        }
    }
}