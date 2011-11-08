namespace Sakura.Framework.Dependencies
{
    using System;
    using System.Collections.Generic;

    using Sakura.Framework.Registration;

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

        public IEnumerable<Type> GetDependencies(IEnumerable<IRegistrationPolicy> policies)
        {
            return AssemblyLocator.FilterDependencyTypes(this.dependencyTypes, policies);
        }
    }
}