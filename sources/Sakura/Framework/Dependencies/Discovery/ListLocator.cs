namespace Sakura.Framework.Dependencies.Discovery
{
    using System;
    using System.Collections.Generic;

    using Sakura.Framework.Dependencies.Policies;

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

        public IEnumerable<Type> GetDependencies(IEnumerable<IRegistrationPolicy> policies)
        {
            return AssemblyLocator.FilterDependencyTypes(this.dependencyTypes, policies);
        }
    }
}