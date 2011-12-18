namespace Sakura.Framework.Dependencies.Discovery
{
    using System;
    using System.Collections.Generic;

    using Sakura.Bootstrapping;
    using Sakura.Framework.Dependencies.Conventions;

    public class ListLocator : IDependencyLocator
    {
        private readonly Type[] dependencyTypes;

        public ListLocator(params Type[] dependencyTypes)
        {
            if (dependencyTypes == null)
            {
                throw new ArgumentNullException("dependencyTypes");
            }

            this.dependencyTypes = dependencyTypes;
        }

        public IEnumerable<Type> GetDependencies(IEnumerable<IRegistrationConvention> conventions)
        {
            return AssemblyLocator.FilterDependencyTypes(this.dependencyTypes, conventions);
        }
    }
}