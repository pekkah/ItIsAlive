namespace Bootstrapper.Composition.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ListLocator : IDependencyLocator
    {
        private readonly IEnumerable<Type> dependencyTypes;

        public ListLocator(IEnumerable<Type> dependencyTypes)
        {
            if (dependencyTypes == null)
            {
                throw new ArgumentNullException("dependencyTypes");
            }

            this.dependencyTypes = dependencyTypes;
        }

        public ListLocator(params Type[] types)
        {
            if (types == null)
            {
                throw new ArgumentNullException("types");
            }

            dependencyTypes = types.ToArray();
        }

        public IEnumerable<Type> GetDependencies(IEnumerable<IRegistrationConvention> conventions)
        {
            return AssemblyLocator.FilterDependencyTypes(dependencyTypes, conventions);
        }
    }
}