namespace Sakura.Framework.Dependencies.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Sakura.Framework.Dependencies.Conventions;

    public class AssemblyLocator : IDependencyLocator
    {
        private readonly IEnumerable<Assembly> assemblies;

        public AssemblyLocator(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        public IEnumerable<Type> GetDependencies(IEnumerable<IRegistrationConvention> policies)
        {
            return this.assemblies.SelectMany(assembly => FilterDependencyTypes(assembly.GetExportedTypes(), policies));
        }

        internal static IEnumerable<Type> FilterDependencyTypes(
            IEnumerable<Type> types, IEnumerable<IRegistrationConvention> policies)
        {
            if (types == null)
            {
                throw new ArgumentNullException("types");
            }

            return types.Where(dependencyType => IsDependency(dependencyType, policies));
        }

        private static bool IsDependency(Type dependencyType, IEnumerable<IRegistrationConvention> policies)
        {
            // skip non discoverable dependencies
            if (Attribute.IsDefined(dependencyType, typeof(NotDiscoverable)))
            {
                return false;
            }

            return policies.Any(policy => policy.IsMatch(dependencyType));
        }
    }
}